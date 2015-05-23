using System;

namespace Mud.Engine.Runtime.Game
{
    public interface IComponent
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        int Id { get; set; }

        INotificationCenter NotificationCenter { get; }

        void PublishMessage<TMessage>(TMessage message) where TMessage : class, IMessage;

        void SubscribeToMessage<TMessage>(Action<TMessage, ISubscription> callback, Func<TMessage, bool> predicate = null) where TMessage : class, IMessage;

        void UnsubscribeFromMessage<TMessage>() where TMessage : class, IMessage;

        void SetNotificationManager(INotificationCenter notificationManager);
    }
}
