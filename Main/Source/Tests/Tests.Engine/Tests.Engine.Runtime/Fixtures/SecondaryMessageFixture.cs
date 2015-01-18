using Mud.Engine.Runtime.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Engine.Runtime.Fixtures
{
    public class SecondaryMessageFixture : MessageBase<IGameComponent>
    {
        public SecondaryMessageFixture(IGameComponent content)
        {
            this.Content = content;
        }
    }
}
