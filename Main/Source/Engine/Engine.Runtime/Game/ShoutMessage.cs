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
    public class ShoutMessage : MessageBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShoutMessage"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ShoutMessage(string message)
        {
            this.Content = message;
        }
    }
}
