//-----------------------------------------------------------------------
// <copyright file="WorldLoadedArgs.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    using System;
    using Mud.Engine.Runtime.Game.Environment;

    /// <summary>
    /// Provides a reference to a World that was loaded.
    /// </summary>
    public class WorldLoadedArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorldLoadedArgs"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        public WorldLoadedArgs(DefaultWorld world)
        {
            this.World = world;
        }

        /// <summary>
        /// Gets the world that was just loaded.
        /// </summary>
        public DefaultWorld World { get; private set; }
    }
}
