using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime
{
    /// <summary>
    /// The mediator for all messaging
    /// </summary>
    public static class MessageCenter
    {
        /// <summary>
        /// Collection of subscribed listeners
        /// </summary>
        private static Dictionary<Type, SubscriptionHandler> listeners =
            new Dictionary<Type, SubscriptionHandler>();

        /// <summary>
        /// Subscribe publications for the message type specified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static SubscriptionHandler Subscribe<T>() where T : class, IMessage
        {
            var handler = new ChatMessageHandler();
            listeners.Add(typeof(T), handler);

            return handler;
        }

        /// <summary>
        /// Publishes the specified message to all subscribers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        public static void Publish<T>(T message) where T : class, IMessage
        {
            if (!listeners.ContainsKey(typeof(T)))
            {
                return;
            }

            SubscriptionHandler handler = listeners[typeof(T)];
            message.Dispatch(handler);
        }
    }

    /// <summary>
    /// Provides a contract to Types wanting to subscribe to published messages 
    /// with conditions and a callback.
    /// </summary>
    public interface SubscriptionHandler
    {
        SubscriptionHandler If(Func<IMessage, bool> condition);

        SubscriptionHandler Dispatch(Action<IMessage> message);
    }

    /// <summary>
    /// Processes a subscription message.
    /// </summary>
    /// <typeparam name="TMessageType">The type of the message type.</typeparam>
    public interface ISubscriptionProcessor<TMessageType> : SubscriptionHandler
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
        public SubscriptionHandler Dispatch(Action<IMessage> message)
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
        public SubscriptionHandler If(Func<IMessage, bool> condition)
        {
            this.conditions.Add(condition);
            return this;
        }

        /// <summary>
        /// Processes the message by verifying the callbacks can be invoked, then invoking them.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ProcessMessage(ChatMessage message)
        {
            // If any of the conditions fail, don't process.
            if (!conditions.Any(condition => condition(message)))
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
        void Dispatch(SubscriptionHandler handler);
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
        public void Dispatch(SubscriptionHandler handler)
        {
            // We must convert ourself to our generic type.
            var msg = this as TMessageType;
            if (msg == null)
            {
                return;
            }

            // Dispatch ourself strongly typed to a protected version
            // of the Dispatch method. 
            this.Dispatch(handler as ISubscriptionProcessor<TMessageType>, msg);
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
            if (target == null)
            {
                return;
            }

            // Let the handler process this message.
            target.ProcessMessage(message);
        }
    }
}
