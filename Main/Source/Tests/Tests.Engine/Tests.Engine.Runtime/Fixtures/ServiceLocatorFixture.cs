using Mud.Engine.Runtime.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Engine.Runtime.Fixtures
{
    internal class ServiceLocatorFixture : IServiceLocator
    {
        internal Func<Type, object> Factory;

        public T Resolve<T>()
        {
            return (T)Factory(typeof(T));
        }

        public void SetLocatorFactory(Func<Type, object> factory)
        {
            this.Factory = factory;
        }
    }
}
