using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Engine.Runtime.Game
{
    public class ProcessCommandRequestMessage : IMessage<string>
    {
        public ProcessCommandRequestMessage(ICharacter sender, string commandToProcess)
        {
            this.Content = commandToProcess;
            this.Sender = sender;
        }

        public string Content { get; private set; }

        public ICharacter Sender { get; private set; }

        public object GetContent()
        {
            return this.Content;
        }
    }
}
