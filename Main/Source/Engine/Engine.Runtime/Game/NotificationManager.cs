﻿//-----------------------------------------------------------------------
// <copyright file="NotificationManager.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <summary>
    /// The mediator for all messaging
    /// </summary>
    public class NotificationManager : INotificationCenter
    {
        /// <summary>
        /// Collection of subscribed listeners
        /// </summary>
        private ConcurrentDictionary<Type, List<ISubscription>> listeners =
            new ConcurrentDictionary<Type, List<ISubscription>>();

        private object listenerCollectionLock = new object();

        /// <summary>
        /// Subscribe publications for the message type specified.
        /// </summary>
        /// <typeparam name="TMessageType"></typeparam>
        /// <returns></returns>
        public ISubscription Subscribe<TMessageType>(Action<TMessageType, ISubscription> callback, Func<TMessageType, bool> condition = null) where TMessageType : class, IMessage
        {
            ExceptionFactory.ThrowIf(
                callback == null,
                () => new ArgumentNullException(nameof(callback), "Callback must not be null when subscribing"));

            Type messageType = typeof(TMessageType);

            // Create our key if it doesn't exist along with an empty collection as the value.
            if (!listeners.ContainsKey(messageType))
            {
                listeners.TryAdd(messageType, new List<ISubscription>());
            }

            // Add our notification to our listener collection so we can publish to it later, then return it.
            // TODO: Move instancing the Notification in to a Factory.
            var handler = new Notification<TMessageType>();
            handler.Register(callback, condition);
            handler.Unsubscribing += this.Unsubscribe;

            List<ISubscription> subscribers = listeners[messageType];
            lock (listenerCollectionLock)
            {
                subscribers.Add(handler);
            }

            return handler;
        }

        /// <summary>
        /// Publishes the specified message to all subscribers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        public void Publish<T>(T message) where T : class, IMessage
        {
            ExceptionFactory.ThrowIf(
                message == null,
                () => new ArgumentNullException(nameof(message), "You can not publish a null message."));

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
        private void Unsubscribe(NotificationArgs args)
        {
            // If the key doesn't exist or has an empty collection we just return.
            // We will leave the key in there for future subscriptions to use.
            if (!listeners.ContainsKey(args.MessageType) || listeners[args.MessageType].Count == 0)
            {
                return;
            }

            // Remove the subscription from the collection associated with the key.
            List<ISubscription> subscribers = listeners[args.MessageType];
            lock (listenerCollectionLock)
            {
                subscribers.Remove(args.Subscription);
            }

            args.Subscription.Unsubscribing -= this.Unsubscribe;
        }
    }
}
