using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game;
using Mud.Engine.Runtime.Game.Character;
using Mud.Engine.Runtime.Networking;

namespace Mud.Apps.Windows.Desktop.Server.App
{
    public abstract class ServerBootstrap
    {
        /// <summary>
        /// The server we are bootstrapping.
        /// </summary>
        public IServer Server { get; set; }

        /// <summary>
        /// The Server configuration setup.
        /// </summary>
        protected IServerConfiguration Configuration { get; set; }

        /// <summary>
        /// Initializes the bootstrapper.
        /// This method will configure the services required to run the server and ensure it is configured.
        /// </summary>
        /// <returns></returns>
        public async Task Initialize()
        {
            this.RegisterAssemblies();
            this.ConfigureServices();
            await this.FinalizeConfiguration();
        }

        /// <summary>
        /// Loads the collection of assemblies in to memory for use.
        /// </summary>
        /// <param name="assemblies"></param>
        protected virtual void RegisterAssemblies()
        {
            Assembly.Load(new AssemblyName("MudDesigner.Engine"));
        }

        protected abstract void Run();

        protected abstract void ConfigureServices();

        protected abstract IServerConfiguration CreateServerConfiguration();

        protected abstract IServer CreateServer();

        protected abstract IGame CreateGame();

        protected abstract void RegisterAllowedSecurityRoles(IEnumerable<ISecurityRole> roles);

        private async Task FinalizeConfiguration()
        {
            // Configure the server
            this.Configuration = this.CreateServerConfiguration();

            // Setup the server security, create the server and start it up.
            IEnumerable<ISecurityRole> roles = await this.Configuration.InitializeSecurityRoles();
            this.RegisterAllowedSecurityRoles(roles);
            await this.PrepareServerForRunning();
        }

        private async Task PrepareServerForRunning()
        {
            this.Server = this.CreateServer();
            IGame game = this.CreateGame();
            await game.Initialize();
            await this.Server.Start(game, this.Configuration);
            this.Run();
        }
    }
}
