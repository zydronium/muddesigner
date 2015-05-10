using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Mud.Apps.Windows.Desktop.Server.App;
using Mud.Data.Shared;
using Mud.Engine.Runtime.Game;
using Mud.Engine.Runtime.Game.Character;
using Mud.Engine.Runtime.Networking;
using Mud.Engine.Runtime.Services;

namespace Mud.Engine.Components.WindowsServer
{
    public class DefaultServerBootstrap : ServerBootstrap
    {
        private IContainer container;

        private List<string> assemblies = new List<string>();

        protected override void Run()
        {
            while (this.Server.Status != ServerStatus.Stopped)
            {
                Thread.Sleep(100);
            }
        }

        protected override void RegisterAssemblies()
        {
            base.RegisterAssemblies();
            Assembly.Load("MudDesigner.Data.Shared");
            Assembly.Load("MudDesigner.Data.FlatFile");
        }

        protected override void ConfigureServices()
        {
            var builder = new ContainerBuilder();
            List<Type> typeCollection = GetTypesToRegister();

            // Runtime Services
            builder.RegisterType<CommandManager>().As<ICommandManager>();
            builder.RegisterType<NotificationManager>().As<INotificationCenter>().SingleInstance();
            builder.RegisterTypes(typeCollection.Where(type => type.IsAssignableTo<IInputCommand>()).ToArray()).AsImplementedInterfaces();

            // Marker Services
            builder.RegisterTypes(typeCollection.Where(type => type.IsAssignableTo<IRepository>()).ToArray()).AsImplementedInterfaces();
            builder.RegisterTypes(typeCollection.Where(type => type.IsAssignableTo<IService>()).ToArray()).AsImplementedInterfaces();
            builder.RegisterTypes(typeCollection.Where(type => type.IsAssignableTo<IComponent>()).ToArray()).AsImplementedInterfaces();

            // Server Services
            builder.RegisterType<DefaultServer>().As<IServer>();
            builder.RegisterType<ServerConfiguration>().As<IServerConfiguration>();

            container = builder.Build();
            
            CharacterFactory.SetFactory(game => container.Resolve<IPlayer>());
        }

        protected override IServerConfiguration CreateServerConfiguration()
        {
            return this.container.Resolve<IServerConfiguration>();
        }

        protected override IServer CreateServer()
        {
            return this.container.Resolve<IServer>();
        }

        protected override IGame CreateGame()
        {
            return this.container.Resolve<IGame>();
        }

        protected override void RegisterAllowedSecurityRoles(IEnumerable<ISecurityRole> roles)
        {
            var builder = new ContainerBuilder();
            foreach (ISecurityRole role in roles)
            {
                builder.RegisterInstance(role).AsImplementedInterfaces();
            }

            builder.Update(this.container);
        }

        private List<Type> GetTypesToRegister()
        {
            var assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(assem =>
                    !assem.FullName.StartsWith("System") &&
                    !assem.FullName.StartsWith("Microsoft") &&
                    !assem.FullName.StartsWith("mscorlib") &&
                    !assem.FullName.StartsWith("vshost") &&
                    !assem.FullName.StartsWith("Autofac"));

            var typeCollection = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                typeCollection.AddRange(assembly.GetTypes());
            }

            return typeCollection;
        }
    }
}
