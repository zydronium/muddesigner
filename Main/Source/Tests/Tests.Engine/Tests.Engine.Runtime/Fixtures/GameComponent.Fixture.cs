using Mud.Engine.Runtime.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Engine.Runtime.Fixtures
{
    internal class GameComponentFixture : GameComponent
    {
        internal Func<Task> LoadDelegate;

        internal Func<Task> UnloadDelegate;

        protected override async Task Load()
        {
            await LoadDelegate();
        }

        protected override async Task Unload()
        {
            await UnloadDelegate();
        }
    }
}
