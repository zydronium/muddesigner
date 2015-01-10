using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime
{
    public interface INotificationCenter
    {
        ISubscriptionHandler Subscribe<T>() where T : class, IMessage;

        void Publish<T>(T message) where T : class, IMessage;
    }

    /// <summary>
    /// The mediator for all messaging
    /// </summary>
    public class ChatCenter : INotificationCenter
    {
        /// <summary>
        /// Collection of subscribed listeners
        /// </summary>
        private Dictionary<Type, List<ISubscriptionHandler>> listeners =
            new Dictionary<Type, List<ISubscriptionHandler>>();

        private static ChatCenter _centerSingleton = new ChatCenter();

        private ChatCenter()
        {
        }

        /// <summary>
        /// Subscribe publications for the message type specified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISubscriptionHandler Subscribe<T>() where T : class, IMessage
        {
            Type messageType = typeof(T);

            if (!listeners.ContainsKey(messageType))
            {
                listeners.Add(messageType, new List<ISubscriptionHandler>());
            }

            // TODO: Move instancing of the handler in to a factory that does a lookup on <T> and returns the right handler.
            var handler = new ChatMessageHandler();
            listeners[messageType].Add(handler);

            return handler;
        }

        public static ChatCenter CurrentCenter
        {
            get
            {
                return _centerSingleton;
            }
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

        internal void Unsubscribe<T>(ISubscriptionHandler handler) where T : class, IMessage
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

    /// <summary>
    /// Provides a contract to Types wanting to subscribe to published messages 
    /// with conditions and a callback.
    /// </summary>
    public interface ISubscriptionHandler
    {
        ISubscriptionHandler If(Func<IMessage, bool> condition);

        ISubscriptionHandler Dispatch(Action<IMessage> message);

        void Unsubscribe();
    }

    /// <summary>
    /// Processes a subscription message.
    /// </summary>
    /// <typeparam name="TMessageType">The type of the message type.</typeparam>
    public interface ISubscriptionProcessor<TMessageType> : ISubscriptionHandler
    {
        void ProcessMessage(TMessageType message);
    }

    /// <summary>
    /// Handles chat message subscriptions
    /// </summary>
    public class ChatMessageHandler : ISubscriptionProcessor<ChatMessage>
    {
        private List<Action<IMessage>> callbacks = new List<Action<IMessage>>();

        private List<Func<IMessage, bool>> conditions = new List<Func<IMessage, bool>>();

        /// <summary>
        /// Registers a callback for when a chat message is published by the MessageCenter
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public ISubscriptionHandler Dispatch(Action<IMessage> message)
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
        public ISubscriptionHandler If(Func<IMessage, bool> condition)
        {
            this.conditions.Add(condition);
            return this;
        }

        public void Unsubscribe()
        {
            this.callbacks.Clear();
            this.conditions.Clear();
            ChatCenter.CurrentCenter.Unsubscribe<ChatMessage>(this);
        }

        /// <summary>
        /// Processes the message by verifying the callbacks can be invoked, then invoking them.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ProcessMessage(ChatMessage message)
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

    /// <summary>
    /// A contract for objects wanting to dispatch message notifications.
    /// </summary>
    public interface IMessage
    {
        void Dispatch(ISubscriptionHandler handler);
    }

    /// <summary>
    /// A chat message.
    /// </summary>
    public class ChatMessage : MessageBase<ChatMessage>
    {
        public ChatMessage(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }
    }

    /// <summary>
    /// Provides methods for dispatching notifications to subscription handlers
    /// </summary>
    /// <typeparam name="TMessageType">The type of the message type.</typeparam>
    public class MessageBase<TMessageType> : IMessage where TMessageType : class, IMessage
    {
        /// <summary>
        /// Dispatches this message instance to the given handler for processing.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void Dispatch(ISubscriptionHandler handler)
        {
            // We must convert ourself to our generic type.
            var msg = this as TMessageType;
            if (msg == null)
            {
                return;
            }

            var target = handler as ISubscriptionProcessor<TMessageType>;
            if (target == null)
            {
                return;
            }

            // Dispatch ourself strongly typed to a protected version
            // of the Dispatch method.
            this.Dispatch(target, msg);
        }

        /// <summary>
        /// Dispatches the given message to the given handler.
        /// Children classes can override this method to perform custom dispatching
        /// if needed.
        /// </summary>
        /// <param name="target">The handler.</param>
        /// <param name="message">The message.</param>
        protected virtual void Dispatch(ISubscriptionProcessor<TMessageType> target, TMessageType message)
        {
            // Let the handler process this message.
            target.ProcessMessage(message);
        }
    }
}
