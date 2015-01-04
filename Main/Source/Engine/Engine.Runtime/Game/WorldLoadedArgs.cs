using Mud.Engine.Runtime.Game.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game
{
    public class WorldLoadedArgs : EventArgs
    {
        public WorldLoadedArgs(DefaultWorld world)
        {
            this.World = world;
        }

        public DefaultWorld World { get; private set; }
    }
}
