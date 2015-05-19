using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character
{
    public interface IInputCommand
    {
        bool IsAsyncCommand { get; }

        bool CanExecuteCommand(ICharacter owner, string command, params string[] args);

        void Execute(ICharacter owner, string command, params string[] args);

        Task ExecuteAsync(ICharacter owner, string command, params string[] args);
    }
}
