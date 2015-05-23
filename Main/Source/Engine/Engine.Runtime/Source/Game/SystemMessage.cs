using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game
{
    public class SystemMessage : IMessage<string>
    {
        public SystemMessage(string message)
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
