using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime
{
    public interface ICloneableComponent<T>
    {
        T Clone();
    }
}
