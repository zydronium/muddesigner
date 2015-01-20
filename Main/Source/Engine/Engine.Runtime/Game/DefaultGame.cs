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
    public class DefaultGame : GameComponent, IGame
    {
        private ILoggingService loggingService;

        private IWorldService worldService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultGame" /> class.
        /// </summary>
        /// <param name="loggingService">The logging service.</param>
        /// <param name="worldService">The world service.</param>
        public DefaultGame()
        {
            ExceptionFactory.
                ThrowIf<ArgumentNullException>(loggingService == null, "Logging service can not be null.", this)
                .Or(worldService == null, "World service can not be null.");

            this.worldService = ServiceLocator.CreateLocator().Resolve<IWorldService>();

            this.Information = new GameInformation();
            this.Autosave = new Autosave<DefaultGame>(this, this.SaveWorlds) { AutoSaveFrequency = 1 };
        }

        /// <summary>
        /// Occurs when a world is loaded, prior to initialization of the world.
        /// </summary>
        public event Func<DefaultGame, WorldLoadedArgs, Task> WorldLoaded;

        /// <summary>
        /// Gets information pertaining to the game.
        /// </summary>
        public GameInformation Information { get; protected set; }

        /// <summary>
        /// Gets the Autosaver responsible for automatically saving the game at a set interval.
        /// </summary>
        public Autosave<DefaultGame> Autosave { get; protected set; }

        /// <summary>
        /// Gets a value indicating that the initialized or not.
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Gets or sets the last saved.
        /// </summary>
        public DateTime LastSaved { get; }

        /// <summary>
        /// Gets or sets the current World for the game. Contains all of the Realms, Zones and Rooms.
        /// </summary>
        public ICollection<DefaultWorld> Worlds { get; set; } = new List<DefaultWorld>();

        /// <summary>
        /// The initialize method is responsible for restoring the world and state.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        protected async override Task Load()
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
                    await world.Initialize();
                }
            }

            await this.Autosave.Initialize();
            this.IsRunning = true;
        }

        /// <summary>
        /// Called when the game is deleted.
        /// Handles clean up of the autosave timer, saving the game state and cleaning up objects.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        protected override async Task Unload()
        {
            await this.Autosave.Delete();

            var worldsToSave = this.Worlds.ToArray();
            await this.SaveWorlds();

            foreach (var world in worldsToSave)
            {
                // Let the world perform clean up and notify it's subscribers it is going away.
                // world.Delete();
                this.Worlds.Remove(world);
            }
        }

        /// <summary>
        /// Occurs when a world is loaded, prior to initialization of the world.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <returns>Returns an awaitable Task</returns>
        protected virtual async Task OnWorldLoaded(DefaultWorld world)
        {
            var handler = this.WorldLoaded;
            if (handler == null)
            {
                return;
            }

            await handler(this, new WorldLoadedArgs(world));
        }

        /// <summary>
        /// Saves each World within the worlds collecton.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        private async Task SaveWorlds()
        {
            foreach (DefaultWorld world in this.Worlds)
            {
                await this.worldService.SaveWorld(world);
            }
        }
    }
}
