//-----------------------------------------------------------------------
// <copyright file="EngineTimer.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Handles chat message subscriptions
    /// </summary>
    public class ChatMessageHandler<T> : INotificationHandler<T> where T : ChatMessage
    {
        /// <summary>
        /// The callbacks invoked when the handler processes the messages.
        /// </summary>
        private List<Action<T>> callbacks = new List<Action<T>>();

        /// <summary>
        /// The conditions that must be met in order to fire the callbacks.
        /// </summary>
        private List<Func<T, bool>> conditions = new List<Func<T, bool>>();

        /// <summary>
        /// Registers a callback for when a chat message is published by the MessageCenter
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public INotificationHandler<T> Register(Action<T> message)
        {
            this.callbacks.Add(message);
            return this;
        }

        /// <summary>
        /// Provides conditional values that will be evaluated upon a publish from the MessageCenter.
        /// If any of the results return false, the callbacks will not be dispatched.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public INotificationHandler<T> If(Func<T, bool> condition)
        {
            this.conditions.Add(condition);
            return this;
        }

        /// <summary>
        /// Unsubscribes the handler from notifications. This cleans up all of the callback references and conditions.
        /// </summary>
        public void Unsubscribe()
        {
            this.callbacks.Clear();
            this.conditions.Clear();

            // Let the notification manager know we are unsubscribing.
            NotificationManager.CurrentCenter.Unsubscribe<ChatMessage>(this);
        }

        /// <summary>
        /// Processes the message by verifying the callbacks can be invoked, then invoking them.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ProcessMessage(T message)
        {
            // If any of the conditions fail, don't process.
            if (conditions.Any(condition => !condition(message)))
            {
                return;
            }

            // Invoke each callback.
            foreach (var callback in this.callbacks)
            {
                callback(message);
            }
        }
    }
}
