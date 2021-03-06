﻿//-----------------------------------------------------------------------
// <copyright file="ServerConnectionEventArgs.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Networking
{
    using Mud.Engine.Runtime.Game.Character;
    using System;

    /// <summary>
    /// Used when a Connection event is fired from the server.
    /// </summary>
    public class ServerConnectionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConnectionEventArgs"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        public ServerConnectionEventArgs(IPlayer player)
        {
            this.Player = player;
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        public IPlayer Player { get; private set; }
    }
}
