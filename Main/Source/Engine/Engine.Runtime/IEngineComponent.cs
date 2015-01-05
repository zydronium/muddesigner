using Mud.Engine.Runtime.Game;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime
{
    public interface IEngineComponent
    {
        Task Initialize(DefaultGame game);
    }
}
