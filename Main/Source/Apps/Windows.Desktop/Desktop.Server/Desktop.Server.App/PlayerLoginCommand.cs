using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Apps.Windows.Desktop.Server.App
{
    public class PlayerLoginCommand : IInputCommand
    {
        private enum LoginState
        {
            FetchingCharacterName,
            FetchingPassword,
            Completed,
        }

        private LoginState currentState = LoginState.FetchingCharacterName;

        public bool IsAsyncCommand
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Command
        {
            get
            {
                return "Login";
            }
        }

        public bool CanExecuteCommand(ICharacter owner, string command, params string[] args)
        {
            return true;
        }

        public InputCommandResult Execute(ICharacter owner, string command, params string[] args)
        {
            InputCommandResult result = null;
            switch(this.currentState)
            {
                case LoginState.FetchingCharacterName:
                    result = new InputCommandResult("Please enter a character name:", false, this, owner);
                    this.currentState = LoginState.FetchingPassword;
                    break;
                case LoginState.FetchingPassword:
                    result = new InputCommandResult("Please enter your password:", true, this, owner);
                    this.currentState = LoginState.Completed;
                    break;
            }

            return result;
        }

        public Task<InputCommandResult> ExecuteAsync(ICharacter owner, string command, params string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
