using Mud.Engine.Runtime;
using Mud.Engine.Runtime.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Apps.Windows.Desktop.Server.App
{
    public class ServiceLocator : IServiceLocator
    {
        private Func<Type, object> serviceFactory;

        public T Resolve<T>()
        {
            ExceptionFactory.ThrowIf<InvalidOperationException>(
                serviceFactory == null,
                "No Locator Factory setup. The app must set up a factory first.");

            return (T)this.serviceFactory(typeof(T));
        }

        public void SetLocatorFactory(Func<Type, object> factory)
        {
            this.serviceFactory = factory;
        }
    }
}
