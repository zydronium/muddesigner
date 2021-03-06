﻿//-----------------------------------------------------------------------
// <copyright file="ActionFromTask.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a Task for delegates that need to invoke an Action
    /// </summary>
    public class TaskFromAction
    {
        /// <summary>
        /// Invokes the given action and returns a Task when completed.
        /// </summary>
        /// <param name="actionToReturnTask">The action to return task.</param>
        /// <returns></returns>
        public static Task Invoke(Action actionToReturnTask)
        {
            ExceptionFactory.ThrowIf(
                actionToReturnTask == null,
                () => new ArgumentNullException(nameof(actionToReturnTask), "Action must not be null."));

            actionToReturnTask();
            return Task.FromResult(true);
        }
    }
}
