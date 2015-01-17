//-----------------------------------------------------------------------
// <copyright file="ChatMessage.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    /// <summary>
    /// A chat message.
    /// </summary>
    public abstract class ChatMessage : MessageBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessage"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ChatMessage(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; private set; }
    }
}
