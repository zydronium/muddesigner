//-----------------------------------------------------------------------
// <copyright file="ChatMessageNotification.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    using System;

    /// <summary>
    /// Handles chat message subscriptions
    /// </summary>
    public class ChatMessageNotification<TMessage> : INotification<TMessage> where TMessage : ChatMessage
    {
        /// <summary>
        /// The callbacks invoked when the handler processes the messages.
        /// </summary>
        private Action<TMessage, ISubscription> callback;

        /// <summary>
        /// The conditions that must be met in order to fire the callbacks.
        /// </summary>
        private Func<TMessage, bool> condition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageNotification{TMessage}"/> class.
        /// </summary>
        /// <param name="notificationCenter">The notification center.</param>
        public ChatMessageNotification(INotificationCenter notificationCenter)
        {
            this.NotificationManager = notificationCenter;
        }

        /// <summary>
        /// Gets the notification manager.
        /// </summary>
        public INotificationCenter NotificationManager { get; private set; }

        /// <summary>
        /// Registers a callback for when a chat message is published by the MessageCenter
        /// </summary>
        /// <param name="processor">The message.</param>
        /// <returns></returns>
        public void Register(
            Action<TMessage, ISubscription> processor,
            Func<TMessage, bool> condition)
        {
            this.callback = processor;
            this.condition = condition;
        }

        /// <summary>
        /// Unsubscribes the handler from notifications. This cleans up all of the callback references and conditions.
        /// </summary>
        public void Unsubscribe()
        {
            this.callback = null;
            this.condition = null;

            // Let the notification manager know we are unsubscribing.
            this.NotificationManager.Unsubscribe<TMessage>(this);
        }

        /// <summary>
        /// Processes the message by verifying the callbacks can be invoked, then invoking them.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ProcessMessage(TMessage message)
        {
            if (!CanProcess(message))
            {
                return;
            }

            Post(message);
        }

        /// <summary>
        /// Determines whether this instance can post notifications with the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private bool CanProcess(TMessage message)
        {
            // If any of the conditions fail, don't process.
            return this.condition(message);
        }

        /// <summary>
        /// Posts the specified message as a notification to the callback.
        /// </summary>
        /// <param name="message">The message.</param>
        private void Post(TMessage message)
        {
            this.callback(message, this);
        }
    }
}
