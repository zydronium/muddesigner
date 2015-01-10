//-----------------------------------------------------------------------
// <copyright file="NotificationManager.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The mediator for all messaging
    /// </summary>
    public class NotificationManager : INotificationCenter
    {
        /// <summary>
        /// Collection of subscribed listeners
        /// </summary>
        private Dictionary<Type, List<ISubscription>> listeners =
            new Dictionary<Type, List<ISubscription>>();

        /// <summary>
        /// The singleton instance for the NotificationManager
        /// </summary>
        private static NotificationManager _centerSingleton = new NotificationManager();

        /// <summary>
        /// Prevents a default instance of the <see cref="NotificationManager"/> class from being created.
        /// </summary>
        private NotificationManager()
        {
        }

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static NotificationManager CurrentCenter
        {
            get
            {
                return _centerSingleton;
            }
        }

        /// <summary>
        /// Subscribe publications for the message type specified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public INotificationHandler<T> Subscribe<T>() where T : class, IMessage
        {
            Type messageType = typeof(T);

            // Create our key if it doesn't exist along with an empty collection as the value.
            if (!listeners.ContainsKey(messageType))
            {
                listeners.Add(messageType, new List<ISubscription>());
            }

            // Create a new handler for <T> from the notification handler factory.
            var handlerFactory = new NotificationHandlerFactory<T>();
            var handler = handlerFactory.CreateNotificationHandlerForMessage();

            // Add our handler to our listener collection so we can publish to it later, then return it.
            listeners[messageType].Add(handler);
            return handler;
        }

        /// <summary>
        /// Publishes the specified message to all subscribers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        public void Publish<T>(T message) where T : class, IMessage
        {
            if (!listeners.ContainsKey(typeof(T)))
            {
                return;
            }

            foreach (var handler in listeners[typeof(T)])
            {
                message.Dispatch(handler);
            }
        }

        /// <summary>
        /// Unsubscribes the specified handler by removing their handler from our collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The handler.</param>
        internal void Unsubscribe<T>(ISubscription handler) where T : class, IMessage
        {
            Type messageType = typeof(T);
            if (!listeners.ContainsKey(messageType))
            {
                return;
            }
            else if (listeners[messageType].Count == 0)
            {
                listeners.Remove(messageType);
                return;
            }

            listeners[messageType].Remove(handler);
        }
    }
}
