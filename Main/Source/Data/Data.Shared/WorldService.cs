﻿using Mud.Engine.Runtime.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Environment;

namespace Mud.Data.Shared
{
    public class WorldService : IWorldService
    {
        private IWorldRepository worldRepository;

        public WorldService(IWorldRepository worldRepository)
        {
            this.worldRepository = worldRepository;
        }

        public async Task<IEnumerable<DefaultWorld>> GetAllWorlds()
        {
            return await this.worldRepository.GetWorlds();
        }
    }
}
