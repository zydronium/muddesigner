using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character.InputCommands
{
    public abstract class CommandProcess
    {
        private CommandProcess nextProcess;

        public CommandProcess()
        {
        }

        public CommandProcess(CommandProcess nextProcess)
        {
            this.nextProcess = nextProcess;
        }

        public CommandProcess GetNextProcessor()
        {
            return this.nextProcess;
        }

        public abstract bool Process(ICharacter owner, IInputCommand command, string[] args, out InputCommandResult result);
    }
}
