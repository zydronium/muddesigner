using Data.Shared;
using Mud.Engine.Shared.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.FlatFile
{
    public class WorldRepository : IWorldRepository
    {
        public Task<IEnumerable<IWorld>> GetWorlds()
        {
            throw new NotImplementedException();
        }
    }
}
