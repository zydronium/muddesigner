using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Services
{
    public static class ServiceLocatorFactory
    {
        private static IServiceLocator _serviceLocator;

        public static void Initialize<TServiceLocator>(TServiceLocator serviceLocator) where TServiceLocator : class, IServiceLocator
        {
            _serviceLocator = serviceLocator;
        }

        public static IServiceLocator CreateServiceLocator()
        {
            return _serviceLocator;
        }
    }
}
