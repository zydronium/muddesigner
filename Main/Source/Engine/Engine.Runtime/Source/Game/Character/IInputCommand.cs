using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character
{
    public interface IInputCommand
    {
        bool IsAsyncCommand { get; }

        string Command { get; }

        bool CanExecuteCommand(ICharacter owner, string command, params string[] args);

        InputCommandResult Execute(ICharacter owner, string command, params string[] args);

        Task<InputCommandResult> ExecuteAsync(ICharacter owner, string command, params string[] args);
    }
}
