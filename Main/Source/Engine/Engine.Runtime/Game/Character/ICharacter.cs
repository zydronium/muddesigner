//-----------------------------------------------------------------------
// <copyright file="ICharacter.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game.Character
{
    using Mud.Engine.Runtime.Game.Environment;
    using System;

    /// <summary>
    /// Defines what makes up a Character within the game.
    /// </summary>
    public interface ICharacter : IGameComponent
    {
        /// <summary>
        /// Occurs when the instance receives an IMessage.
        /// </summary>
        event EventHandler<InputArgs> MessageReceived;

        /// <summary>
        /// Occurs when the instance sends an IMessage.
        /// </summary>
        event EventHandler<InputArgs> MessageSent;

        /// <summary>
        /// Occurs when the character changes rooms.
        /// </summary>
        event EventHandler<OccupancyChangedEventArgs> RoomChanged;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the game.
        /// </summary>
        IGame Game { get; set; }

        /// <summary>
        /// Gets or sets the current room that this character occupies.
        /// </summary>
        DefaultRoom CurrentRoom {get;set;}

        /// <summary>
        /// Moves this character to the given room going in the specified direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="newRoom">The new room.</param>
        void Move(ITravelDirection direction, DefaultRoom newRoom);
    }
}