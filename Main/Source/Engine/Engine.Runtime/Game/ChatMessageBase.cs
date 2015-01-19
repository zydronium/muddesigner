using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game
{
    public abstract class ChatMessageBase : IChatMessage
    {
        public ChatMessageBase(string message, IGameComponent sender)
        {
            this.Content = message;
            this.Sender = sender;
        }

        public string Content { get; protected set; }

        public IGameComponent Sender { get; protected set; }

        public object GetContent()
        {
            return this.GetContent();
        }
    }
}
