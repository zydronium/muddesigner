using Mud.Engine.Runtime.Game.Character;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Components.WindowsServer
{
    public class PlayerConnectionState
    {
        private readonly int bufferSize;

        private readonly List<string> currentData = new List<string>();

        private string lastChunk = string.Empty;

        public PlayerConnectionState(IPlayer player, Socket currentSocket, int bufferSize)
        {
            this.Player = player;
            this.CurrentSocket = currentSocket;

            this.bufferSize = bufferSize;
            this.Buffer = new byte[bufferSize];
        }

        public IPlayer Player { get; private set; }

        public Socket CurrentSocket { get; private set; }

        public byte[] Buffer { get; private set; }

        public bool IsConnectionValid
        {
            get
            {
                return this.CurrentSocket != null && this.CurrentSocket.Connected;
            }
        }

        public bool IsAllDataDelivered
        {
            get
            {
                return this.currentData.Any() && this.currentData.Last().EndsWith("\r\n");
            }
        }

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
            if (!this.IsConnectionValid)
            {
                this.CurrentSocket?.Dispose();
                return;
            }

            // The input string
            int bytesRead = this.CurrentSocket.EndReceive(result);

            if (bytesRead == 0 || !this.Buffer.Any())
            {
                this.StartListeningForData();
                return;
            }

            string data = Encoding.ASCII.GetString(this.Buffer, 0, bytesRead);

            // Temporary to avoid handling the telnet negotiations for now. 
            // This needs to be abstracted out in to a negotation class that will parse, send and receive negotiation requests.
            if (this.Buffer.First() == 255)
            {
                this.StartListeningForData();
                return;
            }

            if (!data.Contains("\r\n"))
            {
                // Add this to our incomplete data stash and read again.
                this.currentData.Add(data);
                this.StartListeningForData();
                return;
            }
            
            List<string> lines = data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            // Append the first line to the incomplete line given to us during the last pass.
            if (this.currentData.Any() && lines.Any())
            {
                lines[0] = string.Format("{0} {1}", string.Join(" ", this.currentData), lines[0]);
                this.currentData.Clear();
            }

            if (lines.Count > 1 && !lines.Last().EndsWith("\r\n"))
            {
                // Stash our incomplete line so we can append to it the next time around.
                this.currentData.Add(lines.Last());
                lines.Remove(lines.Last());
            }

            if (lines.Count == 0)
            {
                this.StartListeningForData();
                return;
            }

            foreach(string line in lines)
            {
                Debug.Write($"Message received: {line}\n");
            }

            this.StartListeningForData();
        }
    }
}
