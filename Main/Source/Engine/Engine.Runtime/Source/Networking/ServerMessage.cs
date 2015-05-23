using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game;

namespace Mud.Engine.Runtime.Networking
{
    public class ServerMessage : IMessage<string>
    {
        public ServerMessage(string message)
        {
            this.Content = message;
        }

        public string Content { get; private set; }

        public object GetContent()
        {
            return this.Content;
        }
    }
}
