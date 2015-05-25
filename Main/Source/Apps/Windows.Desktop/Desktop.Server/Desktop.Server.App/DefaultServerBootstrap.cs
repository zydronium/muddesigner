using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Mud.Apps.Windows.Desktop.Server.App;
using Mud.Data.Shared;
using Mud.Engine.Runtime;
using Mud.Engine.Runtime.Game;
using Mud.Engine.Runtime.Game.Character;
using Mud.Engine.Runtime.Game.Character.InputCommands;
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
            this.Server.GetCurrentGame().NotificationCenter.Subscribe<ServerMessage>((msg, sub) => Console.Write(msg.Content));
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
            Assembly.Load("MudDesigner.Engine.Components.WindowsServer");
        }

        protected override void ConfigureServices()
        {
            var builder = new ContainerBuilder();
            List<Type> typeCollection = GetTypesToRegister();

            // Runtime Services
            builder.RegisterType<CommandManager>().As<ICommandManager>();
            builder.RegisterType<NotificationManager>().As<INotificationCenter>().SingleInstance();
            builder.RegisterTypes(typeCollection.Where(type => type.IsAssignableTo<IInputCommand>()).ToArray())
                .AsImplementedInterfaces()
                .AsSelf();

            // Marker Services
            builder.RegisterTypes(typeCollection.Where(type => type.IsAssignableTo<IRepository>()).ToArray()).AsImplementedInterfaces();
            builder.RegisterTypes(typeCollection.Where(type => type.IsAssignableTo<IService>()).ToArray()).AsImplementedInterfaces();
            builder
                .RegisterTypes(typeCollection.Where(type => type.IsAssignableTo<IComponent>()).ToArray())
                .AsImplementedInterfaces();
            builder
                .RegisterTypes(typeCollection.Where(type => type.IsAssignableTo<IGameComponent>()).ToArray())
                .AsImplementedInterfaces()
                .OnActivating(args => ((IGameComponent)args.Instance).SetNotificationManager(this.container.Resolve<INotificationCenter>()));

            // Server Services
            builder.RegisterType<DefaultServer>().As<IServer>();
            builder.RegisterType<ServerConfiguration>().As<IServerConfiguration>();

            container = builder.Build();

            CommandManagerFactory.SetFactory(() => container.Resolve<ICommandManager>());
            CharacterFactory.SetFactory(
                game => this.container.Resolve<IPlayer>(new TypedParameter(typeof(IGame), game)));

            CommandFactory.SetFactory(CreateCommandFromAlias);
        }

        protected override IServerConfiguration CreateServerConfiguration()
        {
            return this.container.Resolve<IServerConfiguration>();
        }

        protected override IServer CreateServer()
        {
            var server = this.container.Resolve<IServer>();
            server.PlayerConnected += this.ExecuteInitialCommand;
            return server;
        }

        private IInputCommand CreateCommandFromAlias(string alias)
        {
            // Get all of the IInputCommand Types that have been registered.
            IEnumerable<Type> commandTypes = this.container.ComponentRegistry.Registrations
                .Where(service => typeof(IInputCommand).IsAssignableFrom(service.Activator.LimitType))
                .Select(service => service.Activator.LimitType);

            Type commandToInstance = commandTypes
                .FirstOrDefault(type => TypePool.GetAttribute<CommandAliasAttribute>(
                    type: type,
                    property: null,
                    predicate: attribute => attribute.Alias.ToLower().Equals(alias.ToLower())) != null);

            return commandToInstance == null ? null : this.container.Resolve(commandToInstance) as IInputCommand;
        }

        private void ExecuteInitialCommand(object sender, ServerConnectionEventArgs e)
        {
            e.Player.CommandManager.ProcessCommandForCharacter(
                CommandFactory.CreateCommandFromAlias("Login"), 
                new string[0]);
        }

        protected override IGame CreateGame()
        {
            return this.container.Resolve<IGame>();
        }

        protected override IInputCommand InitialConnectionCommand()
        {
            return this.container.Resolve<PlayerLoginCommand>();
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
