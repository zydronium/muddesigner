using Mud.Engine.Runtime.Game.Environment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mud.Data.Shared
{

    public interface IWorldRepository
    {
        Task<IEnumerable<DefaultWorld>> GetWorlds();
    }
}