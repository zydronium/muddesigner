//-----------------------------------------------------------------------
// <copyright file="NotificationManager.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// The mediator for all messaging
    /// </summary>
    public class NotificationManager : INotificationCenter
    {
        /// <summary>
        /// Collection of subscribed listeners
        /// </summary>
        private ConcurrentDictionary<Type, ConcurrentBag<ISubscription>> listeners =
            new ConcurrentDictionary<Type, ConcurrentBag<ISubscription>>();

        /// <summary>
        /// Subscribe publications for the message type specified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISubscription Subscribe<T>(INotification<T> handler) where T : class, IMessage
        {
            Type messageType = typeof(T);

            // Create our key if it doesn't exist along with an empty collection as the value.
            if (!listeners.ContainsKey(messageType))
            {
                listeners.TryAdd(messageType, new ConcurrentBag<ISubscription>());
            }

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

            foreach (INotification<T> handler in listeners[typeof(T)])
            {
                handler.ProcessMessage(message);
            }
        }

        /// <summary>
        /// Unsubscribes the specified handler by removing their handler from our collection.
        /// </summary>
        /// <typeparam name="T">The message Type you want to unsubscribe from</typeparam>
        /// <param name="subscription">The subscription to unsubscribe.</param>
        public void Unsubscribe<T>(ISubscription subscription) where T : class, IMessage
        {
            Type messageType = typeof(T);

            // If the key doesn't exist or has an empty collection we just return.
            // We will leave the key in there for future subscriptions to use.
            if (!listeners.ContainsKey(messageType) || listeners[messageType].Count == 0)
            {
                return;
            }

            // Remove the subscription from the collection associated with the key.
            listeners[messageType].TryTake(out subscription);
        }
    }
}
