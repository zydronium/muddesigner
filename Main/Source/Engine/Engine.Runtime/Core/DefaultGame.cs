﻿//-----------------------------------------------------------------------
// <copyright file="DefaultGame.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Mud.Engine.Shared.Environment;
    using Mud.Engine.Shared.Core;
    using Mud.Engine.Shared.Services;

    /// <summary>
    /// The Default engine implementation of the IGame interface. This implementation provides validation support via ValidationBase.
    /// </summary>
    public class DefaultGame : IGame
    {
        private ILoggingService loggingService;

        private IWorldService worldService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultGame" /> class.
        /// </summary>
        /// <param name="loggingService">The logging service.</param>
        /// <param name="worldService">The world service.</param>
        public DefaultGame(ILoggingService loggingService, IWorldService worldService)
        {
            ExceptionFactory
                .ThrowExceptionIf<ArgumentNullException>(worldService == null, () => new ArgumentNullException("worldService", "World Service must not be null!"))
                .ElseDo(() => this.worldService = worldService);

            ExceptionFactory
                .ThrowExceptionIf<ArgumentNullException>(loggingService == null, () => new ArgumentNullException("loggingService", "Logging ServiBce must not be null!"))
                .ElseDo(() => this.loggingService = loggingService);
        
            this.Version = new Version("0.0.0.1");
        }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is multiplayer.
        /// </summary>
        public bool IsMultiplayer { get; set; }

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
        public Version Version { get; set; }

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
        /// Gets or sets the last saved.
        /// </summary>
        public DateTime LastSaved { get; set; }

        /// <summary>
        /// Gets or sets the current World for the game. Contains all of the Realms, Zones and Rooms.
        /// </summary>
        public ICollection<IWorld> Worlds { get; set; }

        /// <summary>
        /// The initialize method is responsible for restoring the world and state.
        /// </summary>
        /// <returns>Returns the Task associated with the await call.</returns>
        public virtual async Task Initialize()
        {
            this.Worlds = new List<IWorld>(await this.worldService.GetAllWorlds(true));

            if (this.Worlds.Count == 0)
            {
                // Log.
            }
        }
    }
}
