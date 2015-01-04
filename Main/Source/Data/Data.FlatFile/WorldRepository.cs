using Data.Shared;
using Mud.Data.Shared;
using Mud.Engine.Runtime;
using Mud.Engine.Runtime.Game.Environment;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mud.Data.FlatFile
{
    public class WorldRepository : IWorldRepository, ICacheable
    {
        private IFileStorageService storageService;

        private ITimeOfDayStateRepository timeOfDayStateRepository;

        private static ConcurrentBag<DefaultWorld> worldCache = new ConcurrentBag<DefaultWorld>();

        public WorldRepository(IFileStorageService storageService, ITimeOfDayStateRepository timeOfDayRepository)
        {
            this.storageService = storageService;
            this.timeOfDayStateRepository = timeOfDayRepository;
        }

        public async Task<IEnumerable<DefaultWorld>> GetWorlds()
        {
            // Return our previously cached worlds.
            if (WorldRepository.worldCache.Count > 0)
            {
                return new List<DefaultWorld>(WorldRepository.worldCache);
            }

            IEnumerable<string> worldFiles = await this.storageService.GetAllFilesByExtension(".world", "Worlds");
            var worlds = new List<DefaultWorld>();

            foreach (string file in worldFiles)
            {
                var world = new DefaultWorld();
                world.CreationDate = Convert.ToDateTime(await this.storageService.LoadValueFromKeyAsync(
                    file, 
                    world.GetPropertyName(p => p.CreationDate)));

                // Restore the worlds Time of Day States
                IEnumerable<TimeOfDayState> availableStates = await this.timeOfDayStateRepository.GetTimeOfDayStates();
                IEnumerable<string> worldStates = await this.storageService.LoadMultipleValuesFromKeyAsync(
                    file, 
                    world.GetPropertyName(p => p.TimeOfDayStates));
                world.TimeOfDayStates = availableStates.Where(state => worldStates.Any(worldState => worldState == state.Name));

                // Restore the previous time of day state of the world
                // TODO: Some time look at preserving not just the state, but the state.CurrentTime as well.
                string currentTimeOfDayState = await this.storageService.LoadValueFromKeyAsync(
                    file, 
                    world.GetPropertyName(p => p.CurrentTimeOfDay));
                world.CurrentTimeOfDay = world.TimeOfDayStates.First(state => state.Name == currentTimeOfDayState);

                world.GameDayToRealHourRatio = Convert.ToInt32(
                    await this.storageService.LoadValueFromKeyAsync(
                        file, 
                        world.GetPropertyName(p => p.GameDayToRealHourRatio)));

                world.HoursPerDay = Convert.ToInt32(
                    await this.storageService.LoadValueFromKeyAsync(
                        file, 
                        world.GetPropertyName(p => p.HoursPerDay)));

                world.Id = Convert.ToInt32(
                    await this.storageService.LoadValueFromKeyAsync(
                        file,
                        world.GetPropertyName(p => p.Id)));

                world.IsEnabled = Convert.ToBoolean(
                    await this.storageService.LoadValueFromKeyAsync(
                        file,
                        world.GetPropertyName(p => p.IsEnabled)));

                world.Name = await this.storageService.LoadValueFromKeyAsync(
                    file,
                    world.GetPropertyName(p => p.Name));
            }

            WorldRepository.worldCache = new ConcurrentBag<DefaultWorld>(worlds);
            return worlds;
        }

        public void FlushCash()
        {
            WorldRepository.worldCache = new ConcurrentBag<DefaultWorld>();
        }
    }
}
