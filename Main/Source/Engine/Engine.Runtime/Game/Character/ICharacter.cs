using Mud.Engine.Runtime.Game.Environment;
using System;

namespace Mud.Engine.Runtime.Game.Character
{
    public interface ICharacter
    {
        /// <summary>
        /// Occurs when the object is loaded.
        /// </summary>
        event EventHandler Loaded;

        /// <summary>
        /// Occurs when being unloaded.
        /// </summary>
        event EventHandler Unloaded;

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
        /// Gets or sets the unique identifier.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the game.
        /// </summary>
        DefaultGame Game { get; set; }

        /// <summary>
        /// Gets or sets the current room that this character occupies.
        /// </summary>
        DefaultRoom CurrentRoom {get;set;}

        /// <summary>
        /// Initializes this instance with the given game.
        /// </summary>
        /// <param name="game">The game.</param>
        void Initialize(DefaultGame game);
    }
}