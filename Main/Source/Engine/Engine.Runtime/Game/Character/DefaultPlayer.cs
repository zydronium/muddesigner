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
    public class DefaultPlayer : GameComponent, ICharacter, IPlayer
    { 
        private INotificationCenter notificationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPlayer"/> class.
        /// </summary>
        public DefaultPlayer(IGame game)
        {
            this.Game = game;
            this.Information = new CharacterInformation();
            this.Id = 0;
        }

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
        /// Gets the game.
        /// </summary>
        public IGame Game { get; set; }

        /// <summary>
        /// Gets or sets the current room that this character occupies.
        /// </summary>
        public DefaultRoom CurrentRoom { get; set; }

        /// <summary>
        /// Gets or sets information that defines what the character is.
        /// </summary>
        public ICharacterInformation Information { get; set; }

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
        /// <returns>Returns an awaitable Task</returns>
        protected override Task Load()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Unloads this instance and any resources or dependencies it might be using.
        /// Called during deletion of the component.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
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

        protected virtual void OnMessageReceived(InputArgs inputArgs)
        {
            var handler = this.MessageReceived;
            if (handler == null)
            {
                return;
            }

            handler(this, inputArgs);
        }

        protected virtual void OnMessageSent(InputArgs inputArgs)
        {
            var handler = this.MessageSent;
            if (handler == null)
            {
                return;
            }

            handler(this, inputArgs);
        }
    }
}
