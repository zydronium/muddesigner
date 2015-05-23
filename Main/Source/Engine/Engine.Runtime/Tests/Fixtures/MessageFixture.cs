using Mud.Engine.Runtime.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Engine.Runtime.Fixtures
{
    public class MessageFixture : MessageBase<string>
    {
        public MessageFixture(string message)
        {
            this.Content = message;
        }
    }
}
