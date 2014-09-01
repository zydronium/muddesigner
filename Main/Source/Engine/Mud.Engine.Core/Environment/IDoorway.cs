﻿//-----------------------------------------------------------------------
// <copyright file="IDoorway.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Core.Environment
{
    using Mud.Engine.Core.Character;
    using Mud.Engine.Core.Environment.Travel;

    /// <summary>
    /// Provides methods for connecting rooms together.
    /// </summary>
    public interface IDoorway
    {
        /// <summary>
        /// Gets or sets the direction that must be traveled in order to move from the departure room.
        /// </summary>
        ITravelDirection DepartureDirection { get; set; }

        /// <summary>
        /// Gets the arrival room.
        /// </summary>
        IRoom ArrivalRoom { get; }

        /// <summary>
        /// Connects two rooms together.
        /// </summary>
        /// <param name="departureRoom">The departure room.</param>
        /// <param name="arrivalRoom">The arrival room.</param>
        void ConnectRooms(IRoom departureRoom, IRoom arrivalRoom);

        /// <summary>
        /// Disconnects the two connected rooms.
        /// </summary>
        void DisconnectRoom();

        /// <summary>
        /// Moves the given occupant from it's current room to the arrival room associated with this doorway.
        /// </summary>
        /// <param name="occupant">The occupant.</param>
        void TraverseDoorway(ICharacter occupant);
    }
}
