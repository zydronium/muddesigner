//-----------------------------------------------------------------------
// <copyright file="ISubscription.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime
{
    /// <summary>
    /// Provides a contract to Types wanting to subscribe to published messages 
    /// with conditions and a callback.
    /// </summary>
    public interface ISubscription
    {
        /// <summary>
        /// Unsubscribes the registerd callbacks from receiving notifications.
        /// </summary>
        void Unsubscribe();
    }
}
