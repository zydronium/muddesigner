//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Apps.Windows.Desktop.Server.App
{
    using System.Threading;
    using Autofac;
    using Mud.Engine.Runtime.Networking;
    using Mud.Engine.Runtime.Game;
    using Mud.Engine.Components.WindowsServer;
    using Mud.Engine.Runtime.Game.Character;
    using Mud.Data.FlatFile;
    using Mud.Data.Shared;
    using Mud.Apps.Desktop.Windows.ServerApp;
    using Mud.Engine.Runtime.Services;
    using System.Threading.Tasks;

    /// <summary>
    /// The Mud Designer Telnet Server.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The Dependency Injection container
        /// </summary>
        private static IContainer container;

        private static IGame game;

        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            RegisterContainerTypes();

            // Set up the server
            var newServer = container.Resolve<IServer>();
            newServer.Port = 5000;
            newServer.MaxConnections = 100;

            Task startupTask = newServer.Start(container.Resolve<IGame>(), container.Resolve<IServerConfiguration>());
            startupTask.Wait();
            game = newServer.GetCurrentGame();

            // Our game loop.
            while (newServer.Status == ServerStatus.Running)
            {
                Thread.Sleep(500);
            }

            // Check if the server has not stopped. If not, we stop.
            if (newServer.Status != ServerStatus.Stopped)
            {
                // TODO: Convert Server.Stop to Server.Delete to be consistent.
                newServer.Stop();

                // Delete the game.
                Task requestResult = game.Delete();
                requestResult.Wait();
            }
        }

        /// <summary>
        /// Registers the container types.
        /// </summary>
        private static void RegisterContainerTypes()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<LoggingService>().As<ILoggingService>();
            builder.RegisterType<FileStorageService>().As<IFileStorageService>();

            // Engine runtime types
            builder.RegisterType<DefaultPlayer>().As<IPlayer>();
            builder.RegisterType<DefaultGame>().As<IGame>().SingleInstance();
            builder.RegisterType<WorldService>().As<IWorldService>();

            builder.RegisterType<DefaultServer>().As<IServer>();
            builder.RegisterType<ServerConfiguration>().As<IServerConfiguration>();

            // Flat File data store types
            builder.RegisterType<WorldRepository>().As<IWorldRepository>();
            builder.RegisterType<TimeOfDayStateRepository>().As<ITimeOfDayStateRepository>();

            // Notifications
            builder.RegisterType<NotificationManager>().As<INotificationCenter>().SingleInstance();

            builder.RegisterType<ServiceLocator>().As<IServiceLocator>()
                .OnActivating(handler => handler.Instance.SetLocatorFactory(type => container.Resolve(type)));

            // Build our IoC container and use it for the Runtime's ServiceLocator
            container = builder.Build();
            var locator = new ServiceLocator();
            ServiceLocatorFactory.Initialize(locator);
            locator.SetLocatorFactory(type => container.Resolve(type));

            CharacterFactory.SetFactory(game => container.Resolve<IPlayer>());
        }
    }
}
