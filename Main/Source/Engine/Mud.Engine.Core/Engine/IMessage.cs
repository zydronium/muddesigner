﻿//-----------------------------------------------------------------------
// <copyright file="IMessage.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Core.Engine
{
    /// <summary>
    /// Provides a contract for different message types to be stored during validation
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        string Message { get; }
    }
}