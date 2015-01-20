namespace Mud.Engine.Runtime.Services
{
    public static class ServiceLocator
    {
        private static IServiceLocator serviceType;

        public static void Initialize<TServiceLocator>() where TServiceLocator : IServiceLocator, new()
        {
            serviceType = new TServiceLocator();
        }

        public static IServiceLocator CreateLocator()
        {
            return serviceType;
        }
    }
}
