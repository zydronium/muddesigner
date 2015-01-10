using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime
{
    internal class NotificationHandlerFactory<T> where T : class, IMessage
    {
        private static ConcurrentDictionary<T, INotificationHandler<T>> handlerCache =
            new ConcurrentDictionary<T, INotificationHandler<T>>();

        internal INotificationHandler<T> CreateNotificationHandlerForMessage()
        {
            // TODO: Use reflection and cache the results.
            if (typeof(T) == typeof(ChatMessage))
            {
                return new ChatMessageHandler<ChatMessage>() as INotificationHandler<T>;
            }

            return null;
        }
    }
}
