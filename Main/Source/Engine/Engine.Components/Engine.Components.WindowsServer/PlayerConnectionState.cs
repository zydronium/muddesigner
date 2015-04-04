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

            if (bytesRead == 0)
            {
                this.StartListeningForData();
                return;
            }

            string data = Encoding.UTF8.GetString(this.Buffer, 0, bytesRead);
            if (!data.Contains("\r\n"))
            {
                // Add this to our incomplete data stash and read again.
                this.currentData.Add(data);
                this.StartListeningForData();
                return;
            }

            // We have completed at least one line, so parse the data.
            List<string> lines = data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList(); 
            if (this.currentData.Any() && lines.Any())
            {
                lines[0] = string.Join(" ", this.currentData);
                this.currentData.Clear();
            }

            if (!lines.Last().EndsWith("\r\n"))
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
                Debug.WriteLine($"Message received: {line}\n");
            }

            this.StartListeningForData();
        }
    }
}
