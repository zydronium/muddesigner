using Mud.Engine.Shared.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Shared
{

    public interface IWorldRepository
    {
        Task<IEnumerable<IWorld>> GetWorlds();
    }
}
