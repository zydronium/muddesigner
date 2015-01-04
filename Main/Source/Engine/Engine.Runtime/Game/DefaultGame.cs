//-----------------------------------------------------------------------
// <copyright file="DefaultGame.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Mud.Engine.Runtime.Game.Environment;
    using Mud.Engine.Runtime.Services;

    /// <summary>
    /// The Default engine implementation of the IGame interface. This implementation provides validation support via ValidationBase.
    /// </summary>
    public class DefaultGame : GameComponent
    {
        private ILoggingService loggingService;

        private IWorldService worldService;

        private List<Func<Task>> asyncShutdownCallbacks = new List<Func<Task>>();

        private List<Action> shutdownCallbacks = new List<Action>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultGame" /> class.
        /// </summary>
        /// <param name="loggingService">The logging service.</param>
        /// <param name="worldService">The world service.</param>
        public DefaultGame(ILoggingService loggingService, IWorldService worldService)
        {
            this.loggingService = loggingService;
            this.worldService = worldService;
        }

        public event Func<DefaultGame, GameShutdownArgs, Task> ShutdownRequested;

        public event EventHandler ShutdownCompleted;

        public event Func<DefaultGame, WorldLoadedArgs, Task> WorldLoaded;

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the game being played.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the game.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the current version of the game.
        /// </summary>
        public Version Version { get; set; } = new Version("0.0.0.1");

        /// <summary>
        /// Gets or sets the website that users can visit to get information on the game.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [hide room names].
        /// </summary>
        public bool HideRoomNames { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable automatic save].
        /// </summary>
        public bool EnableAutoSave { get; set; }

        /// <summary>
        /// Gets or sets the automatic save frequency in seconds.
        /// </summary>
        public int AutoSaveFrequency { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is multiplayer.
        /// </summary>
        public bool IsMultiplayer { get; set; }

        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Gets or sets the last saved.
        /// </summary>
        public DateTime LastSaved { get; set; }

        /// <summary>
        /// Gets or sets the current World for the game. Contains all of the Realms, Zones and Rooms.
        /// </summary>
        public ICollection<DefaultWorld> Worlds { get; set; } = new List<DefaultWorld>();

        /// <summary>
        /// The initialize method is responsible for restoring the world and state.
        /// </summary>
        /// <returns>Returns the Task associated with the await call.</returns>
        public async override Task Load()
        {
            this.Worlds = new List<DefaultWorld>(await this.worldService.GetAllWorlds());

            if (this.Worlds.Count == 0)
            {
            }
            else
            {
                foreach (DefaultWorld world in this.Worlds)
                {
                    await this.OnWorldLoaded(world);
                    world.Initialize();
                }
            }
        }

        /// <summary>
        /// Attempts to shut down the game. 
        /// This method will fire the ShutdownCompleted event after all of the Delete events have completed.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> RequestShutdown()
        {
            bool isCanceled = false;

            // TODO: Return a cancellation object that holds the sender and member for logging.
            await this.OnShutdownRequested((sender, member) => isCanceled = true);

            if (isCanceled)
            {
                return false;
            }

            await this.Delete();
            this.OnShutdownCompleted();

            return true;
        }

        public override async Task Unload()
        {
            foreach (var world in this.Worlds)
            {
                await this.worldService.SaveWorld(world);

                // Let the world perform clean up and notify it's subscribers it is going away.
                // world.Delete();
            }
        }

        protected virtual async Task OnWorldLoaded(DefaultWorld world)
        {
            var handler = this.WorldLoaded;
            if (handler == null)
            {
                return;
            }

            await handler(this, new WorldLoadedArgs(world));
        }

        protected virtual async Task OnShutdownRequested(Action<IGameComponent, string> cancelCallback)
        {
            Func<DefaultGame, GameShutdownArgs, Task> handler = this.ShutdownRequested;

            if (handler == null)
            {
                return;
            }

            var invocationList = (Func<DefaultGame, GameShutdownArgs, Task>[])handler.GetInvocationList();
            Task[] handlerTasks = new Task[invocationList.Length];

            for (int i = 0; i < invocationList.Length; i++)
            {
                handlerTasks[i] = invocationList[i](this, new GameShutdownArgs(cancelCallback));
            }

            await Task.WhenAll(handlerTasks);
        }

        protected virtual void OnShutdownCompleted()
        {
            var handler = this.ShutdownCompleted;
            if (handler == null)
            {
                return;
            }

            handler(this, new EventArgs());
        }

        private async Task SaveWorlds()
        {
            foreach (DefaultWorld world in this.Worlds)
            {
                await this.worldService.SaveWorld(world);
            }
        }
    }
}
