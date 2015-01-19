//-----------------------------------------------------------------------
// <copyright file="DefaultPlayer.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game.Character
{
    using System;
    using System.Threading.Tasks;
    using Mud.Engine.Runtime;
    using Mud.Engine.Runtime.Game.Environment;

    /// <summary>
    /// The Default Engine implementation of IPlayer.
    /// </summary>
    public class DefaultPlayer : GameComponent, ICharacter
    { 
        /// <summary>
        /// The CurrentRoom property backing field.
        /// </summary>
        private DefaultRoom currentRoom;

        private INotificationCenter chatCenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPlayer"/> class.
        /// </summary>
        public DefaultPlayer(IGame game, INotificationCenter chatCenter)
        {
            this.Game = game;
            this.chatCenter = chatCenter;

            this.Id = 0;
        }

        /// <summary>
        /// Occurs when the object is loaded.
        /// </summary>
        public event EventHandler Loaded;

        /// <summary>
        /// Occurs when being unloaded.
        /// </summary>
        public event EventHandler Unloaded;

        /// <summary>
        /// Occurs when the instance receives an IMessage.
        /// </summary>
        public event EventHandler<InputArgs> MessageReceived;

        /// <summary>
        /// Occurs when the instance sends an IMessage.
        /// </summary>
        public event EventHandler<InputArgs> MessageSent;

        /// <summary>
        /// Occurs when the character changes rooms.
        /// </summary>
        public event EventHandler<OccupancyChangedEventArgs> RoomChanged;

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the game.
        /// </summary>
        public IGame Game { get; set; }

        /// <summary>
        /// Gets or sets the current room that this character occupies.
        /// </summary>
        public DefaultRoom CurrentRoom { get; set; }

        /// <summary>
        /// Moves this character to the given room going in the specified direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="newRoom">The new room.</param>
        public void Move(ITravelDirection direction, DefaultRoom newRoom)
        {
            this.OnRoomChanged(direction, this.CurrentRoom, newRoom);
        }

        /// <summary>
        /// Loads the component and any resources or dependencies it might have.
        /// Called during initialization of the component
        /// </summary>
        /// <returns></returns>
        protected override Task Load()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Unloads this instance and any resources or dependencies it might be using.
        /// Called during deletion of the component.
        /// </summary>
        /// <returns></returns>
        protected override Task Unload()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Called when the character changes rooms.
        /// </summary>
        /// <param name="departingRoom">The departing room.</param>
        /// <param name="arrivalRoom">The arrival room.</param>
        protected virtual void OnRoomChanged(ITravelDirection direction, DefaultRoom departingRoom, DefaultRoom arrivalRoom)
        {
            EventHandler<OccupancyChangedEventArgs> handler = this.RoomChanged;
            if (handler == null)
            {
                return;
            }

            handler(this, new OccupancyChangedEventArgs(this, direction, departingRoom, arrivalRoom));
        }
    }
}
