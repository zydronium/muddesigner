//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Apps.Windows.Desktop.Server.App
{
    using System.Linq;
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
    using System;
    using System.Reflection;
    using System.Collections.Generic;




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
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var typeCollection = new List<Type>();
            foreach(Assembly assembly in assemblies.Where(assem => !assem.FullName.Contains("System.")))
            {
                typeCollection.AddRange(assembly.GetTypes());
            }

            var builder = new ContainerBuilder();
            builder.RegisterTypes(typeCollection.Where(type => type.IsAssignableTo<IService>()).ToArray()).AsImplementedInterfaces();
            builder.RegisterTypes(typeCollection.Where(type => type.IsAssignableTo<IComponent>()).ToArray()).AsImplementedInterfaces();

            // Engine runtime types
            builder.RegisterType<DefaultServer>().As<IServer>();
            builder.RegisterType<ServerConfiguration>().As<IServerConfiguration>();

            // Flat File data store types
            builder.RegisterType<WorldRepository>().As<IWorldRepository>();
            builder.RegisterType<TimeOfDayStateRepository>().As<ITimeOfDayStateRepository>();

            // Notifications
            builder.RegisterType<NotificationManager>().As<INotificationCenter>().SingleInstance();

            // Build our IoC container and use it for the Runtime's ServiceLocator
            container = builder.Build();

            CharacterFactory.SetFactory(game => container.Resolve<IPlayer>());
        }
    }
}
