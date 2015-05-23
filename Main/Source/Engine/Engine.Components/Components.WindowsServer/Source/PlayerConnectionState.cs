//-----------------------------------------------------------------------
// <copyright file="PlayerConnectionState.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Components.WindowsServer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using Mud.Engine.Runtime.Game;
    using Mud.Engine.Runtime.Game.Character;

    /// <summary>
    /// Handles the players networking state.
    /// </summary>
    public sealed class PlayerConnectionState
    {
        /// <summary>
        /// The size of the buffer that will hold data sent from the client
        /// </summary>
        private readonly int bufferSize;

        /// <summary>
        /// A temporary collection of incomplete messages sent from the client. These must be put together and processed.
        /// </summary>
        private readonly List<string> currentData = new List<string>();

        /// <summary>
        /// What the last chunk of data sent from the client contained.
        /// </summary>
        private string lastChunk = string.Empty;

        private INotificationCenter notificationManager;

        private readonly ObservableCollection<string> outgoingMessageQueue = new ObservableCollection<string>();

        private bool isSendingMessage;

        private object sendingMessageLock = new object();

        /// <summary>
        /// Instances a new PlayerConnectionState.
        /// </summary>
        /// <param name="player">An instance of a Player type that will be performing network communication</param>
        /// <param name="currentSocket">The Socket used to communicate with the client.</param>
        /// <param name="bufferSize">The storage size of the data buffer</param>
        public PlayerConnectionState(IPlayer player, Socket currentSocket, int bufferSize)
        {
            this.Player = player;
            this.notificationManager = player.NotificationCenter;
            this.notificationManager.Subscribe<SystemMessage>(
                callback: (msg, sub) => this.SendMessage(msg.Content),
                condition: (msg) => string.IsNullOrEmpty(msg.Content));

            this.CurrentSocket = currentSocket;
            this.bufferSize = bufferSize;
            this.Buffer = new byte[bufferSize];

            this.outgoingMessageQueue.CollectionChanged += MessageAddedToOutboundQueue;
        }

        /// <summary>
        /// Gets the Player instance associated with this state.
        /// </summary>
        public IPlayer Player { get; private set; }

        /// <summary>
        /// Gets the Socket for the player associated with this state.
        /// </summary>
        public Socket CurrentSocket { get; private set; }

        /// <summary>
        /// Gets the data currently in the network buffer
        /// </summary>
        public byte[] Buffer { get; private set; }

        /// <summary>
        /// Gets if the current network connection is in a valid state.
        /// </summary>
        public bool IsConnectionValid
        {
            get
            {
                return this.CurrentSocket != null && this.CurrentSocket.Connected;
            }
        }

        /// <summary>
        /// Starts listening for network communication sent from the client to the server
        /// </summary>
        public void StartListeningForData()
        {
            this.Buffer = new byte[bufferSize];
            this.CurrentSocket.BeginReceive(this.Buffer, 0, bufferSize, 0, new AsyncCallback(this.ReceiveData), null);
        }

        public void SendMessage(string message)
        {
            if (!this.IsConnectionValid)
            {
                return;
            }

            lock (this.outgoingMessageQueue)
            {
                this.outgoingMessageQueue.Add(message);
            }
        }

        private void MessageAddedToOutboundQueue(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.isSendingMessage)
            {
                return;
            }

            if ((e.Action == NotifyCollectionChangedAction.Add && e.NewItems.Count == 0) ||
                e.Action != NotifyCollectionChangedAction.Add)
            {
                return;
            }

            this.ProcessOutboundMessage(e.NewItems[0] as string);
        }

        private void ProcessOutboundMessage(string message)
        {
            this.isSendingMessage = true;
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            this.CurrentSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.CompleteMessageSending), message);
        }

        private void CompleteMessageSending(IAsyncResult asyncResult)
        {
            if (!this.IsConnectionValid)
            {
                return;
            }

            this.CurrentSocket.EndSend(asyncResult);
            string content = asyncResult.AsyncState as string;

            lock(this.sendingMessageLock)
            {
                this.outgoingMessageQueue.Remove(content);
            }

            if (this.outgoingMessageQueue.Any())
            {
                this.ProcessOutboundMessage(this.outgoingMessageQueue.First());
            }
            else
            {
                this.isSendingMessage = false;
            }
        }

        private void HandleCommandExecutionCompleted(object sender, CommandCompletionArgs e)
        {
            if (e == null || e.CommandResult == null || string.IsNullOrEmpty(e.CommandResult.Result) || !this.IsConnectionValid)
            {
                return;
            }

            this.SendMessage($"{e.CommandResult.Result}");
        }

        /// <summary>
        /// Receives the input data from the user.
        /// </summary>
        /// <param name="result">The result.</param>
        private void ReceiveData(IAsyncResult result)
        {
            int bytesRead = this.CurrentSocket.EndReceive(result);

            if (bytesRead == 0 || !this.Buffer.Any())
            {
                this.CurrentSocket.BeginReceive(this.Buffer, 0, bufferSize, 0, new AsyncCallback(this.ReceiveData), null);
                return;
            }

            ProcessReceivedData(bytesRead);

            // If we are no longer in a valid state after we have processed the results, abort.
            if (!this.IsConnectionValid)
            {
                return;
            }

            this.CurrentSocket.BeginReceive(this.Buffer, 0, bufferSize, 0, new AsyncCallback(this.ReceiveData), null);
        }

        /// <summary>
        /// Process the data we received from the client.
        /// </summary>
        /// <param name="bytesRead"></param>
        private void ProcessReceivedData(int bytesRead)
        {
            // Encode our input string sent from the client
            this.lastChunk = Encoding.ASCII.GetString(this.Buffer, 0, bytesRead);

            // Temporary to avoid handling the telnet negotiations for now. 
            // This needs to be abstracted out in to a negotation class that will parse, send and receive negotiation requests.
            if (this.Buffer.First() == 255)
            {
                return;
            }

            // If the previous chunk did not have a new line feed, then we add this message to the collection of currentData.
            // This lets us build a full message before processing it.
            if (!lastChunk.Contains("\r\n"))
            {
                // Add this to our incomplete data stash and read again.
                this.currentData.Add(lastChunk);
                return;
            }

            // This message contained at least 1 new line, so we split it and process per line.
            List<string> messages = lastChunk.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (string line in this.PruneReceivedMessages(messages))
            {
                CommandRequestMessage commandRequest = this.ParseMessage(line);
                this.notificationManager.Publish(commandRequest);
            }
        }

        private CommandRequestMessage ParseMessage(string line)
        {
            // Split up the command and each of it's arguments from the line.
            string[] content = line.Split(' ');

            // Determine what the command and argument values we will provide the CommandRequestMessage.
            string command = string.Empty;
            string[] arguments = new string[0];
            if (content.Length >= 1)
            {
                command = content.First().Trim('\r', '\n');
            }

            if (content.Length > 1)
            {
                // We skip the first element, as that is the command.
                arguments = content.Skip(1).ToArray();
            }

            // Create the request and publish it
            return new CommandRequestMessage(this.Player, command, arguments);
        }


        /// <summary>
        /// Runs through the messages collection and prepends data from a previous, incomplete, message
        /// and updates the internal message tracking state.
        /// </summary>
        /// <param name="messages"></param>
        private List<string> PruneReceivedMessages(List<string> messages)
        {
            // Append the first line to the incomplete line given to us during the last pass if one exists.
            if (this.currentData.Any() && messages.Any())
            {
                messages[0] = string.Format("{0} {1}", string.Join(" ", this.currentData), messages[0]);
                this.currentData.Clear();
            }

            // If we have more than 1 line and the last line in the collection does not end with a line feed
            // then we add it to our current data so it may be completed during the next pass. 
            // We then remove it from the lines collection because it can be infered that the remainder will have
            // a new line due to being split on \n.
            if (messages.Count > 1 && !messages.Last().EndsWith("\r\n"))
            {
                this.currentData.Add(messages.Last());
                messages.Remove(messages.Last());
            }

            return messages;
        }
    }
}
