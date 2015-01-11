//-----------------------------------------------------------------------
// <copyright file="INotificationHandler.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    using System;

    /// <summary>
    /// Processes a subscription message.
    /// </summary>
    /// <typeparam name="TMessageType">The type of the message type.</typeparam>
    public interface INotificationHandler<TMessageType> : ISubscription where TMessageType : class, IMessage
    {
        /// <summary>
        /// Registers a lambda to use when determining if the registered callback should be fired
        /// when a notification is fired for T.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        INotificationHandler<TMessageType> If(Func<TMessageType, bool> condition);

        /// <summary>
        /// Registers the specified action for callback when a notification is fired for T.
        /// </summary>
        /// <param name="message">The message being posted along with the subscription registered to receive the post.</param>
        /// <returns></returns>
        INotificationHandler<TMessageType> Register(Action<TMessageType, ISubscription> message);

        /// <summary>
        /// Processes the message, invoking the registered callbacks if their conditions are met.
        /// </summary>
        /// <param name="message">The message.</param>
        void ProcessMessage(TMessageType message);
    }
}
