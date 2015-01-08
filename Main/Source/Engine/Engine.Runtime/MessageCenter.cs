using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime
{
    public static class MessageCenter
    {
        private static Dictionary<Type, IHandler> listeners =
            new Dictionary<Type, IHandler>();

        public static IHandler Subscribe<T>() where T : class, IMessage
        {
            var handler = new ChatMessageHandler();
            listeners.Add(typeof(T), handler);

            return handler;
        }

        public static void Publish<T>(T message) where T : class, IMessage
        {
            if (!listeners.ContainsKey(typeof(T)))
            {
                return;
            }

            IHandler handler = listeners[typeof(T)];
            message.Dispatch(handler);
        }
    }

    public interface IHandler
    {
        IHandler If(Func<IMessage, bool> condition);

        IHandler Dispatch(Action<IMessage> message);
    }

    public interface IMessageHandler<TMessageType> : IHandler
    {
        void ProcessMessage(TMessageType message);
    }

    public interface IMessage
    {
        void Dispatch(IHandler handler);
    }

    public class ChatMessage : MessageBase<ChatMessage>
    {
        public ChatMessage(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }
    }

    public class MessageBase<TMessageType> : IMessage where TMessageType : class, IMessage
    {
        public Type MessageType
        {
            get
            {
                return typeof(TMessageType);
            }
        }

        public void Dispatch(IHandler handler)
        {
            var msg = this as TMessageType;
            if (msg == null)
            {
                return;
            }

            this.Dispatch(handler, msg);
        }

        protected void Dispatch(IHandler handler, TMessageType message)
        {
            var target = handler as IMessageHandler<TMessageType>;
            if (target == null)
            {
                return;
            }

            target.ProcessMessage(message);
        }
    }

    public class ChatMessageHandler : IMessageHandler<ChatMessage>
    {
        private List<Action<IMessage>> callbacks = new List<Action<IMessage>>();

        private List<Func<IMessage, bool>> conditions = new List<Func<IMessage, bool>>();

        public IHandler Dispatch(Action<IMessage> message)
        {
            this.callbacks.Add(message);
            return this;
        }

        public IHandler If(Func<IMessage, bool> condition)
        {
            this.conditions.Add(condition);
            return this;
        }

        public void ProcessMessage(ChatMessage message)
        {
            if (!conditions.Any(condition => condition(message)))
            {
                return;
            }

            foreach (var callback in this.callbacks)
            {
                callback(message);
            }
        }
    }
}
