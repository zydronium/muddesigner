//-----------------------------------------------------------------------
// <copyright file="INotificationCenter.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    /// <summary>
    /// Provides a contract for Mediators to use when handling notifications between objects.
    /// </summary>
    public interface INotificationCenter
    {
        /// <summary>
        /// Sets up a new handler and returns it for subscription set up.
        /// </summary>
        /// <typeparam name="T">An IMessage implementation that the given handler will be provided when messages are dispatched</typeparam>
        /// <param name="handler">The handler used to process incoming messages.</param>
        /// <returns>Returns an ISubscription that can be used to unsubscribe.</returns>
        ISubscription Subscribe<T>(INotification<T> handler) where T : class, IMessage;

        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        void Publish<T>(T message) where T : class, IMessage;

        /// <summary>
        /// Unsubscribes the given subscription.
        /// </summary>
        /// <typeparam name="T">The IMessage that was subscribed to</typeparam>
        /// <param name="subscription">The subscription.</param>
        void Unsubscribe<T>(ISubscription subscription) where T : class, IMessage;
    }
}
