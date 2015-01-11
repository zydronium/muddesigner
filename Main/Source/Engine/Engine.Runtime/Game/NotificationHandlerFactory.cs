//-----------------------------------------------------------------------
// <copyright file="NotificationHandlerFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    using System.Reflection;

    /// <summary>
    /// An internal Factory used to create instances of INotificationHandler implementations based on the given IMessage implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class NotificationHandlerFactory<T> where T : class, IMessage
    {
        /// <summary>
        /// Creates the notification handler for the message type associated with this Factory.
        /// </summary>
        /// <returns>Returns an implementation of INotificationHandler that supports the specified IMessage</returns>
        internal INotificationHandler<T> CreateNotificationHandlerForMessage()
        {
            // TODO: Use reflection and cache the results.
            if (typeof(T).GetTypeInfo().BaseType == typeof(ChatMessage))
            {
                return new ChatMessageHandler<ChatMessage>() as INotificationHandler<T>;
            }

            return null;
        }
    }
}
