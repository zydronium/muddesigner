using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character
{
    public interface IInputCommand
    {
        bool CanExecuteCommand(ICharacter owner, params string[] args);

        Task<InputCommandResult> ExecuteAsync(ICharacter owner, params string[] args);
    }
}
