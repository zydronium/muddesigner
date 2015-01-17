//-----------------------------------------------------------------------
// <copyright file="ISubscription.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    /// <summary>
    /// Provides a contract to Types wanting to subscribe to published messages 
    /// with conditions and a callback.
    /// </summary>
    public interface ISubscription
    {
        /// <summary>
        /// Gets the notification manager.
        /// </summary>
        INotificationCenter NotificationManager { get; }

        /// <summary>
        /// Unsubscribes the registerd callbacks from receiving notifications.
        /// </summary>
        /// <param name="notificationCenter">The notification center.</param>
        void Unsubscribe();
    }
}
