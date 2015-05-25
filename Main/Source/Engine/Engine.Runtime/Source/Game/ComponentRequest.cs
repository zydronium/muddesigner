using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character
{
    public abstract class ComponentRequest : IMessage<string>
    {
        public ComponentRequest(string message, IComponent sendingCharacter)
        {
            this.Content = message;
            this.Sender = sendingCharacter;
        }

        public string Content { get; private set; }

        public IComponent Sender { get; private set; }

        public object GetContent()
        {
            return this.Content;
        }
    }
}
