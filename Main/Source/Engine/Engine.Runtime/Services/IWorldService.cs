using Mud.Engine.Runtime.Game.Environment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Services
{
    public interface IWorldService : IService
    {
        Task<IEnumerable<IWorld>> GetAllWorlds();

        Task SaveWorld(IWorld world);
    }
}
