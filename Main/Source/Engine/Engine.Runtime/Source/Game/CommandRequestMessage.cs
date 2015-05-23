using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Engine.Runtime.Game
{
    public class CommandRequestMessage : IMessage<string>
    {
        public CommandRequestMessage(ICharacter sender, string commandToProcess, string[] args)
        {
            this.Content = commandToProcess;
            this.Sender = sender;
            this.Arguments = args;
        }

        public string Content { get; private set; }

        public ICharacter Sender { get; private set; }

        public string[] Arguments { get; private set; }

        public object GetContent()
        {
            return this.Content;
        }
    }
}
