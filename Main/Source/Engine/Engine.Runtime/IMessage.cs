//-----------------------------------------------------------------------
// <copyright file="IMessage.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime
{
    /// <summary>
    /// A contract for objects wanting to dispatch message notifications.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Dispatches the specified handler for message processing.
        /// </summary>
        /// <param name="handler">The handler.</param>
        void Dispatch(ISubscription handler);
    }
}
