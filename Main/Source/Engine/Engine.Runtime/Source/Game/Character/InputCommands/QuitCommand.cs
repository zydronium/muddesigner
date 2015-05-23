using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character.InputCommands
{
    [CommandAlias("Quit")]
    [CommandAlias("Disconnect")]
    [CommandAlias("DC")]
    public class QuitCommand : IInputCommand
    {
        public bool CanExecuteCommand(ICharacter owner, params string[] args)
        {
            return owner != null;
        }

        public Task<InputCommandResult> ExecuteAsync(ICharacter owner, params string[] args)
        {
            return owner.Delete().ContinueWith(task => new InputCommandResult(true, this, owner));
        }
    }
}
