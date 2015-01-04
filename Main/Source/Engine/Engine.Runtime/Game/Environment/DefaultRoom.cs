﻿//-----------------------------------------------------------------------
// <copyright file="DefaultRoom.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game.Environment
{
    using System;
    using System.Collections.Generic;
    using Mud.Engine.Runtime.Game.Character;


    /// <summary>
    /// The Default engine Room Type.
    /// </summary>
    public class DefaultRoom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRoom"/> class.
        /// </summary>
        public DefaultRoom()
        {
            this.Doorways = new List<DefaultDoorway>();
            this.Occupants = new List<ICharacter>();
            this.CreationDate = DateTime.Now;
        }

        /// <summary>
        /// Occurs when a character enters the room.
        /// </summary>
        public event EventHandler<OccupancyChangedEventArgs> EnteredRoom;

        /// <summary>
        /// Occurs when a character leaves the room.
        /// </summary>
        public event EventHandler<OccupancyChangedEventArgs> LeftRoom;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets how many seconds have passed since the creation date.
        /// </summary>
        public double TimeFromCreation { get; protected set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the zone that owns this Room.
        /// </summary>
        public DefaultZone Zone { get; protected set; }

        /// <summary>
        /// Gets or sets the doorways that this Room has, linked to other IRooms.
        /// </summary>
        public ICollection<DefaultDoorway> Doorways { get; protected set; }

        /// <summary>
        /// Gets or sets the occupants within this Room..
        /// </summary>
        public ICollection<ICharacter> Occupants { get; protected set; }

        /// <summary>
        /// Initializes the room with the given zone.
        /// </summary>
        /// <param name="zone">The zone that represents the owner of this room.</param>
        public virtual void Initialize(DefaultZone zone)
        {
            this.Zone = zone;
        }

        /// <summary>
        /// Adds the occupant to this instance.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <exception cref="System.NullReferenceException">Attempted to add a null character to the Room.</exception>
        public void AddOccupantToRoom(ICharacter character)
        {
            // We don't allow the user to enter a disabled room.
            if (!this.IsEnabled)
            {
                // TODO: Need to do some kind of communication back to the caller that this can't be traveled to.
                throw new InvalidOperationException("The room is disabled and can not be traveled to.");
            }

            if (character == null)
            {
                throw new NullReferenceException("Attempted to add a null character to the Room.");
            }

            // Remove the character from their previous room.
            character.CurrentRoom.RemoveOccupantFromRoom(character, this);

            this.Occupants.Add(character);
            this.OnEnteringRoom(character, character.CurrentRoom);

            character.CurrentRoom = this;
        }

        /// <summary>
        /// Removes the occupant from room.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="arrivalRoom">The arrival room.</param>
        public void RemoveOccupantFromRoom(ICharacter character, DefaultRoom arrivalRoom)
        {
            if (character == null)
            {
                return;
            }

            this.Occupants.Remove(character);
            this.OnLeavingRoom(character, arrivalRoom);
        }

        /// <summary>
        /// Called when a character enters this room.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="departingRoom">The departing room.</param>
        protected virtual void OnEnteringRoom(ICharacter character, DefaultRoom departingRoom)
        {
            EventHandler<OccupancyChangedEventArgs> handler = this.EnteredRoom;
            if (handler == null)
            {
                return;
            }

            handler(this, new OccupancyChangedEventArgs(character, departingRoom, this));
        }

        /// <summary>
        /// Called when a character leaves this room.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="arrivalRoom">The arrival room.</param>
        protected virtual void OnLeavingRoom(ICharacter character, DefaultRoom arrivalRoom)
        {
            EventHandler<OccupancyChangedEventArgs> handler = this.LeftRoom;
            if (handler == null)
            {
                return;
            }

            handler(this, new OccupancyChangedEventArgs(character, this, arrivalRoom));
        }
    }
}
