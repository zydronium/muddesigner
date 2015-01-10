//-----------------------------------------------------------------------
// <copyright file="MessageBase.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime
{
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
        public void Dispatch(ISubscription handler)
        {
            // We must convert ourself to our generic type.
            var msg = this as TMessageType;
            if (msg == null)
            {
                return;
            }

            var target = handler as INotificationHandler<TMessageType>;
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
        protected virtual void Dispatch(INotificationHandler<TMessageType> target, TMessageType message)
        {
            // Let the handler process this message.
            target.ProcessMessage(message);
        }
    }
}
