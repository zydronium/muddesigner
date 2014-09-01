﻿//-----------------------------------------------------------------------
// <copyright file="AfternoonState.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Core.Environment.Time
{
    /// <summary>
    /// A state representing an afternoon in time.
    /// </summary>
    public class AfternoonState : TimeOfDayState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AfternoonState"/> class.
        /// </summary>
        public AfternoonState()
        {
            this.StateStartTime = new TimeOfDay();
            this.StateStartTime.Hour = 12;
        }

        /// <summary>
        /// Gets the name of this state.
        /// </summary>
        public override string Name 
        { 
            get 
            { 
                return "Afternoon"; 
            } 
        }

        /// <summary>
        /// Gets the time of day in the game that this state begins.
        /// </summary>
        public override TimeOfDay StateStartTime { get; set; }
    }
}
