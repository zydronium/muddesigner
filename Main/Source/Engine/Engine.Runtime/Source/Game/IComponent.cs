using System;

namespace Mud.Engine.Runtime.Game
{
    public interface IComponent
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        Guid Id { get; set; }

        DateTime CreatedDate { get; set; }

        DateTime LastUpdatedDate { get; set; }
    }
}
