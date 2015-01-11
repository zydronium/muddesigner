//-----------------------------------------------------------------------
// <copyright file="AutoSave.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides auto-save support to an object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Autosave<T> : IInitializableComponent
    {
        /// <summary>
        /// The autosave timer
        /// </summary>
        private EngineTimer<T> autosaveTimer;

        /// <summary>
        /// The item to save when the timer fires
        /// </summary>
        private T ItemToSave;

        /// <summary>
        /// The delegate to call when the timer fires
        /// </summary>
        private Func<Task> saveDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Autosave{T}"/> class.
        /// </summary>
        /// <param name="itemToSave">The item to save.</param>
        /// <param name="saveDelegate">The save delegate.</param>
        public Autosave(T itemToSave, Func<Task> saveDelegate)
        {
            ExceptionFactory
                .ThrowIf<ArgumentNullException>(itemToSave == null, "Can not save a null item.")
                .Or(saveDelegate == null, "Save delegate must not be null.");

            this.ItemToSave = itemToSave;
            this.saveDelegate = saveDelegate;
            this.AutoSaveFrequency = 0;
        }

        /// <summary>
        /// Gets or sets the automatic save frequency in Minutes.
        /// Set the frequency to zero in order to disable auto-save.
        /// </summary>
        public int AutoSaveFrequency { get; set; }

        /// <summary>
        /// Gets a value indicating whether the autosave timer is running.
        /// </summary>
        public bool IsAutosaveRunning
        {
            get
            {
                return this.autosaveTimer == null ? false : this.autosaveTimer.IsRunning;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns></returns>
        public Task Initialize()
        {
            // Set up our auto-save if the frequency is set for it.
            if (this.AutoSaveFrequency <= 0)
            {
                return Task.FromResult(false);
            }

            this.autosaveTimer = new EngineTimer<T>(this.ItemToSave);
            double autosaveInterval = TimeSpan.FromMinutes(this.AutoSaveFrequency).TotalMilliseconds;

            this.autosaveTimer.StartAsync(
                autosaveInterval,
                autosaveInterval,
                0,
                (game, timer) => this.saveDelegate());

            return Task.FromResult(true);
        }

        public Task Delete()
        {
            this.autosaveTimer.Stop();
            return Task.FromResult(true);
        }
    }
}
