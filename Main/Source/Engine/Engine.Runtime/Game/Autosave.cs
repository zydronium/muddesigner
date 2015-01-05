using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game
{
    public class Autosave<T> : GameComponent
    {
        /// <summary>
        /// The autosave timer
        /// </summary>
        private EngineTimer<T> autosaveTimer;

        private T ItemToSave;

        private Func<Task> saveDelegate;

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

        protected override Task Load()
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

        protected override Task Unload()
        {
            this.autosaveTimer.Stop();
            return Task.FromResult(true);
        }
    }
}
