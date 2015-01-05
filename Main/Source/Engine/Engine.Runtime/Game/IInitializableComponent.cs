using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game
{
    public interface IInitializableComponent
    {
        Task Initialize();

        Task Delete();
    }
}
