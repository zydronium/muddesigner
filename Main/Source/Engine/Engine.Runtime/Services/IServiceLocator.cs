using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Services
{
    public interface IServiceLocator
    {
        void SetLocatorFactory(Func<Type, object> factory);

        T Resolve<T>();
    }
}
