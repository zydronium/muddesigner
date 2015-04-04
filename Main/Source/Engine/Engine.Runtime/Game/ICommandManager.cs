using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Engine.Runtime.Game
{
    public interface ICommandManager : IInitializableComponent
    {
        Task ProcessCommandForCharacter(ICharacter character, string command);
    }
}
