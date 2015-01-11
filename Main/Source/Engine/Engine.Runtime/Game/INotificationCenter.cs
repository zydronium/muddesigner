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
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        INotificationHandler<T> Subscribe<T>() where T : class, IMessage;

        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        void Publish<T>(T message) where T : class, IMessage;
    }
}
