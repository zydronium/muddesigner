﻿//-----------------------------------------------------------------------
// <copyright file="ServerStatus.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Shared.Networking
{
    /// <summary>
    /// Provides status values for the server to use, indicating its current state.
    /// </summary>
    public enum ServerStatus
    {
        /// <summary>
        /// The server has stopped.
        /// </summary>
        Stopped,

        /// <summary>
        /// Server is in the process of starting.
        /// </summary>
        Starting,

        /// <summary>
        /// Server is up and running.
        /// </summary>
        Running
    }
}
