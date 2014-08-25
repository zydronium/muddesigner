﻿//-----------------------------------------------------------------------
// <copyright file="DefaultWorld.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Core.Environment
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The Default World class used by the engine.
    /// </summary>
    public class DefaultWorld : IWorld
    {
        /// <summary>
        /// The time of day states
        /// </summary>
        private List<ITimeOfDayState> timeOfDayStates;

        private List<IRealm> realms;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWorld"/> class.
        /// </summary>
        public DefaultWorld()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.Now;

            // Set the in-game day to be 24 hours, with each day taking 45 real-world minutes to cycle.
            this.HoursPerDay = 24;
            this.GameDayToRealHourRatio = 0.75;

            // Set up default states for the time of day.
            this.timeOfDayStates = new List<ITimeOfDayState> { new MorningState(), new AfternoonState(), new NightState() };
            this.realms = new List<IRealm>();

            // Must be in the constructor. If assigned within the Initialization method
            // the property could potentially never be restored properly from the data store.
            this.CreationDate = DateTime.Now;
        }

        /// <summary>
        /// Occurs when [time of day changed].
        /// </summary>
        public event EventHandler<TimeOfDayChangedEventArgs> TimeOfDayChanged;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets how many seconds have passed since the creation date.
        /// </summary>
        /// <value>
        /// The time from creation.
        /// </value>
        public double TimeFromCreation { get { return this.CreationDate.Subtract(DateTime.Now).TotalSeconds; } }

        /// <summary>
        /// Gets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the current time of day.
        /// </summary>
        /// <value>
        /// The current time of day.
        /// </value>
        public ITimeOfDayState CurrentTimeOfDay { get; set; }

        /// <summary>
        /// Gets or sets a collection of states that can be used for the time of day.
        /// </summary>
        /// <value>
        /// The time of day states.
        /// </value>
        public IEnumerable<ITimeOfDayState> TimeOfDayStates
        {
            get
            {
                return this.timeOfDayStates;
            }

            set
            {
                this.timeOfDayStates.Clear();

                if (value != null)
                {
                    this.timeOfDayStates.AddRange(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets hour many hours it takes in-game to complete 1 day.
        /// </summary>
        /// <value>
        /// The hours per day.
        /// </value>
        public int HoursPerDay { get; set; }

        /// <summary>
        /// Gets or sets the hours ratio. If set to 4, it takes 4 in-game hours to equal 1 real-world hour.
        /// </summary>
        /// <value>
        /// The hours ratio.
        /// </value>
        public double GameDayToRealHourRatio { get; set; }

        /// <summary>
        /// Gets the game time ratio used to convert real-world time to game-time.
        /// </summary>
        /// <value>
        /// The game time ratio.
        /// </value>
        public double GameTimeAdjustmentFactor { get { return this.GameDayToRealHourRatio / this.HoursPerDay; } }

        /// <summary>
        /// Gets or sets the realms in this world.
        /// </summary>
        /// <value>
        /// The realms.
        /// </value>
        public IEnumerable<IRealm> Realms
        {
            get
            {
                return this.realms;
            }

            set
            {
                this.realms.Clear();

                if (value != null)
                {
                    this.realms.AddRange(value);
                }
            }
        }

        /// <summary>
        /// Initializes the world by starting the world clock and the associated Realm clocks.
        /// </summary>
        public virtual void Initialize(ITimeOfDayState initialState = null)
        {
            // Set up our time of day clock.
            if (this.timeOfDayStates.Count > 0 && initialState == null)
            {
                // If we do not have an initial state, then we create a state based on our current real-world time.
                this.SetupWorldClock(this.GetTimeOfDayState(DateTime.Now));
            }
            else if (initialState != null)
            {
                // If an initial state is provided, then we hand it off to the setup method.
                SetupWorldClock(initialState);
            }

            // Notify listeners that our time of day has changed.
            this.OnTimeOfDayChanged(null, this.CurrentTimeOfDay);
        }

        /// <summary>
        /// Adds the given realm to world and initializes it.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <exception cref="System.NullReferenceException">
        /// Attempted to add a null Realm to the world.
        /// or
        /// Adding a Realm to a World with a null Zones collection is not allowed.
        /// </exception>
        public void AddRealmToWorld(IRealm realm)
        {
            if (realm == null)
            {
                throw new NullReferenceException("Attempted to add a null Realm to the world.");
            }

            if (realm.Zones == null)
            {
                throw new NullReferenceException("Adding a Realm to a World with a null Zones collection is not allowed.");
            }

            realm.CreationDate = DateTime.Now;
            realm.Initialize(this);

            this.realms.Add(realm);
        }

        /// <summary>
        /// Adds the time of day state to the world.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <exception cref="System.InvalidOperationException">You can not have two states with the same start time in the same world.</exception>
        /// <exception cref="System.NullReferenceException">State's start time must not be null.</exception>
        public void AddTimeOfDayState(ITimeOfDayState state)
        {
            // We prevent states with the same start time from being added to the world
            if (this.TimeOfDayStates.Any(s => s.StateStartTime.Equals(state)))
            {
                throw new InvalidOperationException("You can not have two states with the same start time in the same world.");
            }

            // Null start times engine from transitioning to the new state.
            if (state.StateStartTime == null)
            {
                throw new NullReferenceException("State's start time must not be null.");
            }

            this.timeOfDayStates.Add(state);
        }

        /// <summary>
        /// Called when the current time of day has changed and a new one is transitioning in.
        /// </summary>
        /// <param name="oldTimeOfDay">The old time of day.</param>
        /// <param name="newTimeOfDay">The new time of day.</param>
        protected virtual void OnTimeOfDayChanged(ITimeOfDayState oldTimeOfDay, ITimeOfDayState newTimeOfDay)
        {
            // Our event handler
            EventHandler<TimeOfDayChangedEventArgs> handler = this.TimeOfDayChanged;
            if (handler == null)
            {
                return;
            }

            // Invoke the handler
            handler(this, new TimeOfDayChangedEventArgs(oldTimeOfDay, newTimeOfDay));
        }

        /// <summary>
        /// Sets up the world clock.
        /// </summary>
        /// <param name="initialState">The initial state.</param>
        private void SetupWorldClock(ITimeOfDayState initialState)
        {
            // We want to reset our current state before we set up the next state
            // The next state starts on a background thread and can cause listeners to access
            // the old state before the new state is assigned preventing a proper reset.
            if (this.CurrentTimeOfDay != null)
            {
                this.CurrentTimeOfDay.TimeUpdated -= this.CurrentTimeOfDay_TimeUpdated;
                this.CurrentTimeOfDay.Reset();
            }

            // Register for event updates
            initialState.TimeUpdated += this.CurrentTimeOfDay_TimeUpdated;

            // Initialize the state.
            initialState.Initialize(this.GameTimeAdjustmentFactor, this.HoursPerDay);

            this.CurrentTimeOfDay = initialState;
        }

        /// <summary>
        /// Event handler method fired when the Current TimeOfDay state has its time changed.
        /// </summary>
        /// <param name="sender">The TimeOfDayState that caused the time change.</param>
        /// <param name="e">The new time of day.</param>
        private void CurrentTimeOfDay_TimeUpdated(object sender, TimeOfDay e)
        {
            // We need to check if we have exceeded the time available in a given day.
            if (e.Hour > this.HoursPerDay)
            {
                e.Hour = 0;
                e.Minute = e.Minute;
            }

            // Check the current in-game time and see if we need to move to a new state or not.
            ITimeOfDayState newTimeOfDay = this.GetTimeOfDayState(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, e.Hour, e.Minute, 0));
            if (this.CurrentTimeOfDay == newTimeOfDay)
            {
                // State is the same, so we just return.
                return;
            }

            Debug.WriteLine(String.Format("Transitioned to {0} state.", newTimeOfDay.Name));

            // We need to store a reference to the current state, since the Setup method overwrites it.
            var currentTimeOfDay = this.CurrentTimeOfDay;

            // Set up the new state.
            this.SetupWorldClock(newTimeOfDay);

            // Invoke our event handler with the old state and the new state.
            this.OnTimeOfDayChanged(currentTimeOfDay, newTimeOfDay);
        }


        /// <summary>
        /// Looks at a supplied time of day and figures out what TimeOfDayState needs to be returned that matches the time of day.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns></returns>
        private ITimeOfDayState GetTimeOfDayState(DateTime? currentTime = null)
        {
            ITimeOfDayState inProgressState = null;
            ITimeOfDayState nextState = null;

            TimeOfDay time = new TimeOfDay();
            time.Hour = currentTime.Value.Hour;
            time.Minute = currentTime.Value.Minute;

            Parallel.Invoke(
                () => inProgressState = this.GetInProgressState(time),
                () => nextState = this.GetNextState(time));

            if (inProgressState != null)
            {
                return inProgressState;
            }
            else if (nextState != null && nextState.StateStartTime.Hour <= time.Hour && nextState.StateStartTime.Minute <= time.Minute)
            {
                return nextState;
            }

            return this.CurrentTimeOfDay;
        }

        /// <summary>
        /// Gets a state if there is one already in progress.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns></returns>
        private ITimeOfDayState GetInProgressState(TimeOfDay currentTime)
        {
            ITimeOfDayState inProgressState = null;
            foreach (ITimeOfDayState state in this.TimeOfDayStates)
            {
                // If the state is already in progress, w
                if (state.StateStartTime.Hour <= currentTime.Hour ||
                    (state.StateStartTime.Hour <= currentTime.Hour && state.StateStartTime.Minute <= currentTime.Minute))
                {
                    if (inProgressState == null)
                    {
                        inProgressState = state;
                        continue;
                    }
                    else
                    {
                        if (inProgressState.StateStartTime.Hour <= currentTime.Hour &&
                            inProgressState.StateStartTime.Minute <= currentTime.Minute)
                        {
                            inProgressState = state;
                        }
                    }
                }
            }

            return inProgressState;
        }

        /// <summary>
        /// Gets the state that is up next.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns></returns>
        private ITimeOfDayState GetNextState(TimeOfDay currentTime)
        {
            ITimeOfDayState nextState = null;
            foreach (ITimeOfDayState state in this.TimeOfDayStates)
            {
                // If this state is a future state, then preserve it as a possible next state.
                if (state.StateStartTime.Hour > currentTime.Hour ||
                    (state.StateStartTime.Hour >= currentTime.Hour && state.StateStartTime.Minute > currentTime.Minute))
                {
                    // If we do not have a next state, set it.
                    if (nextState == null)
                    {
                        nextState = state;
                        continue;
                    }
                    else
                    {
                        // We have a next state, so we must check which is sooner.
                        if (nextState.StateStartTime.Hour > state.StateStartTime.Hour &&
                            nextState.StateStartTime.Minute >= state.StateStartTime.Minute)
                        {
                            nextState = state;
                        }
                    }
                }
            }

            return nextState;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // These should all have their clocks disabled, but we ensure they are anyway.
            // This will also pick up our Current state during the process.
            foreach (ITimeOfDayState state in this.TimeOfDayStates)
            {
                state.Reset();
            }
        }
    }
}