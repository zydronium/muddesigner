using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character
{
    public interface ICharacter
    { /// <summary>
      /// The CurrentRoom property backing field.
      /// </summary>
        private DefaultRoom currentRoom;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPlayer"/> class.
        /// </summary>
        public DefaultPlayer()
        {
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
        public DefaultGame Game { get; private set; }

        /// <summary>
        /// Gets or sets the current room that this character occupies.
        /// </summary>
        public DefaultRoom CurrentRoom
        {
            get
            {
                return this.currentRoom;
            }

            set
            {
                DefaultRoom departingRoom = this.currentRoom;
                this.currentRoom = value;

                this.OnRoomChanged(departingRoom, value);
            }
        }

        /// <summary>
        /// Initializes this instance with the given game.
        /// </summary>
        /// <param name="game">The game.</param>
        public void Initialize(DefaultGame game)
        {
            this.Game = game;

            // TODO: Fetch permissions and handle log-in.
            this.OnLoaded();
        }
    }
}
