using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;
using Mud.Engine.Runtime.Game.Character.InputCommands;

namespace Mud.Apps.Windows.Desktop.Server.App
{
    [CommandAlias("Login")]
    public class PlayerLoginCommand : IInputCommand
    {
        private enum LoginState
        {
            FetchingCharacterName,
            FetchingPassword,
            Completed,
        }

        private LoginState currentState = LoginState.FetchingCharacterName;

        public bool CanExecuteCommand(ICharacter owner, params string[] args)
        {
            return true;
        }

        public Task<InputCommandResult> ExecuteAsync(ICharacter owner, params string[] args)
        {
            InputCommandResult result = null;
            switch (this.currentState)
            {
                case LoginState.FetchingCharacterName:
                    result = new InputCommandResult("Please enter a character name:", false, this, owner);
                    this.currentState = LoginState.FetchingPassword;
                    break;
                case LoginState.FetchingPassword:
                    result = new InputCommandResult("Please enter your password:", false, this, owner);
                    this.currentState = LoginState.Completed;
                    break;
                case LoginState.Completed:
                    result = new InputCommandResult("Logged in.\r\n", true, this, owner);
                    break;
            }

            return Task.FromResult(result);
        }
    }
}
