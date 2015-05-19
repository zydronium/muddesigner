using System.Collections.Generic;

namespace Mud.Engine.Runtime.Game.Character
{
    public class CommandCompletionArgs
    {
        public CommandCompletionArgs(string command, ICharacter owner, IEnumerable<string> args)
        {
            this.Command = command;
            this.Arguments = args;
            this.Owner = owner;
        }

        public string Command { get; private set; }

        public IEnumerable<string> Arguments { get; private set; }

        public ICharacter Owner { get; set; }
    }
}