using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Engine.Runtime.Game
{
    public class CommandRequestMessage : ComponentRequest
    {
        public CommandRequestMessage(ICharacter sender, string commandToProcess, string[] args)
            : base(commandToProcess, sender)
        {
            this.Arguments = args;
        }

        public string[] Arguments { get; private set; }
    }
}
