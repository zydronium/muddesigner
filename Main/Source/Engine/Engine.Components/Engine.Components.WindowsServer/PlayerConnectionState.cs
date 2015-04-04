﻿//-----------------------------------------------------------------------
// <copyright file="PlayerConnectionState.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Components.WindowsServer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using Mud.Engine.Runtime.Game.Character;

    /// <summary>
    /// Handles the players networking state.
    /// </summary>
    public class PlayerConnectionState
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

        /// <summary>
        /// Instances a new PlayerConnectionState.
        /// </summary>
        /// <param name="player">An instance of a Player type that will be performing network communication</param>
        /// <param name="currentSocket">The Socket used to communicate with the client.</param>
        /// <param name="bufferSize">The storage size of the data buffer</param>
        public PlayerConnectionState(IPlayer player, Socket currentSocket, int bufferSize)
        {
            this.Player = player;
            this.CurrentSocket = currentSocket;

            this.bufferSize = bufferSize;
            this.Buffer = new byte[bufferSize];
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

        /// <summary>
        /// Receives the input data from the user.
        /// </summary>
        /// <param name="result">The result.</param>
        private void ReceiveData(IAsyncResult result)
        {
            // If we are no longer in a valid state, dispose of the connection.
            if (!this.IsConnectionValid)
            {
                this.CurrentSocket?.Dispose();
                return;
            }

            int bytesRead = this.CurrentSocket.EndReceive(result);
            if (bytesRead == 0 || !this.Buffer.Any())
            {
                this.StartListeningForData();
                return;
            }

            ProcessReceivedData(bytesRead);
            this.StartListeningForData();
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
                // TODO: Hande off the line to a command manager or some type.
                Debug.Write($"Message received: {line}\n");
            }
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
                // Stash our incomplete line so we can append to it the next time around.
                this.currentData.Add(messages.Last());
                messages.Remove(messages.Last());
            }

            return messages;
        }
    }
}