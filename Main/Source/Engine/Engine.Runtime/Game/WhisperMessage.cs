//-----------------------------------------------------------------------
// <copyright file="WhisperMessage.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    using System;
    using Mud.Engine.Runtime.Game.Character;

    /// <summary>
    /// Used when an object needs to send a private message to a character.
    /// </summary>
    public class WhisperMessage : ChatMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhisperMessage"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="target">The target.</param>
        public WhisperMessage(string message, ICharacter target) : base(message)
        {
            this.Target = target;
        }

        /// <summary>
        /// Gets the target that this message is intended for.
        /// </summary>
        public ICharacter Target { get; private set; }
    }
}
