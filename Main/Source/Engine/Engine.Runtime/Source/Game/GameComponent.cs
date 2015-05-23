//-----------------------------------------------------------------------
// <copyright file="GameComponent.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The root class for all game Types.
    /// </summary>
    public abstract class GameComponent : IGameComponent
    {
        /// <summary>
        /// The Loading event is fired during initialization of the component prior to being loaded.
        /// </summary>
        public event Func<IGameComponent, Task> Loading;

        /// <summary>
        /// The Loaded event is fired upon completion of the components initialization and loading.
        /// </summary>
        public event EventHandler<EventArgs> Loaded;

        /// <summary>
        /// The Deleting event is fired immediately upon a delete request.
        /// </summary>
        public event Func<IGameComponent, Task> Deleting;

        /// <summary>
        /// The Deleted event is fired once the object has finished processing it's unloading and clean up.
        /// </summary>
        public event EventHandler<EventArgs> Deleted;

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Initializes the game component.
        /// </summary>
        /// <returns></returns>
        public async Task Initialize()
        {
            await this.LoadingBegan();
            await this.Load();
            this.LoadingCompleted();
        }

        /// <summary>
        /// Informs the component that it is no longer needed, allowing it to perform clean up.
        /// Objects registered to one of the two delete events will be notified of the delete request.
        /// </summary>
        /// <returns></returns>
        public async Task Delete()
        {
            await this.OnDeleteRequested();
            await this.Unload();
            this.OnDeleted();
        }

        /// <summary>
        /// Loads the component and any resources or dependencies it might have. 
        /// Called during initialization of the component
        /// </summary>
        /// <returns></returns>
        protected abstract Task Load();

        /// <summary>
        /// Unloads this instance and any resources or dependencies it might be using.
        /// Called during deletion of the component.
        /// </summary>
        /// <returns></returns>
        protected abstract Task Unload();

        /// <summary>
        /// This gets called when initialization of the component begins. 
        /// This is called prior to Load() being invoked.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadingBegan()
        {
            var handler = this.Loading;
            if (handler == null)
            {
                return;
            }

            await handler(this);
        }

        /// <summary>
        /// Called when initialization is completed. Provides last-chance initialization support.
        /// Called after Load() has completed.
        /// </summary>
        protected virtual void LoadingCompleted()
        {
            var handler = this.Loaded;
            if (handler == null)
            {
                return;
            }

            handler(this, new EventArgs());
        }

        /// <summary>
        /// This gets called when deletion of the component begins. 
        /// This is called prior to Unload() being invoked.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task OnDeleteRequested()
        {
            var handler = this.Deleting;
            if (handler == null)
            {
                return;
            }

            await handler(this);
        }

        /// <summary>
        /// Called when Deletion is completed. Provides last-chance clean-up support.
        /// Called after Unload() has completed.
        /// </summary>
        protected virtual void OnDeleted()
        {
            var handler = this.Deleted;
            if (handler == null)
            {
                return;
            }

            handler(this, new EventArgs());
        }
    }
}
