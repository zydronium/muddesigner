using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character.InputCommands
{
    public class QuitCommand : IInputCommand
    {
        public string Command
        {
            get
            {
                return "Quit";
            }
        }

        public bool IsAsyncCommand { get { return true; } }

        public bool CanExecuteCommand(ICharacter owner, string command, params string[] args)
        {
            return owner == null || !command.ToLower().Equals(this.Command.ToLower());
        }

        public InputCommandResult Execute(ICharacter owner, string command, params string[] args)
        {
            throw new NotImplementedException();
        }

        public Task<InputCommandResult> ExecuteAsync(ICharacter owner, string command, params string[] args)
        {
            return owner.Delete().ContinueWith(t =>
            {
                return new InputCommandResult("Disconnecting.\r\n", true, this, owner);
            });
        }
    }
}
