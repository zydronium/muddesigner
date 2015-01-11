//-----------------------------------------------------------------------
// <copyright file="GameInformation.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    using System;

    /// <summary>
    /// Provides meta-information for the currently running game.
    /// </summary>
    public class GameInformation : IComponent
    {
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
    }
}
