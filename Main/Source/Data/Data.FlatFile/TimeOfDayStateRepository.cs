using Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Environment;
using Mud.Data.Shared;
using Mud.Engine.Runtime;

namespace Mud.Data.FlatFile
{
    public class TimeOfDayStateRepository : ITimeOfDayStateRepository
    {
        private IFileStorageService storageService;

        public TimeOfDayStateRepository(IFileStorageService fileStorageService)
        {
            this.storageService = fileStorageService;
        }

        public async Task<IEnumerable<TimeOfDayState>> GetTimeOfDayStates()
        {
            IEnumerable<string> stateFiles = this.storageService.GetAllFilesByExtension(".tods", "TimeOfDayStates");
            var states = new List<TimeOfDayState>();

            foreach(string file in stateFiles)
            {
                var state = new TimeOfDayState();
                state.Name = await this.storageService.LoadValueFromKeyAsync(file, state.GetPropertyName(p => p.Name));
                state.StateStartTime.Hour = Convert.ToInt32(await this.storageService.LoadValueFromKeyAsync(
                    file,
                    state.StateStartTime.GetPropertyName(p => p.Hour)));
                state.StateStartTime.Minute = Convert.ToInt32(await this.storageService.LoadValueFromKeyAsync(
                    file, 
                    state.StateStartTime.GetPropertyName(p => p.Minute)));
                state.StateStartTime.HoursPerDay = Convert.ToInt32(await this.storageService.LoadValueFromKeyAsync(
                    file,
                    state.StateStartTime.GetPropertyName(p => p.HoursPerDay)));

                states.Add(state);
            }

            return states;
        }
    }
}
