using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character
{
    public interface IInputCommand
    {
        bool IsAsyncCommand { get; }

        void Execute();

        Task ExecuteAsync();
    }
}
