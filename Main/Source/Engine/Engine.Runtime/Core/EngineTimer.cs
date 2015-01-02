//-----------------------------------------------------------------------
// <copyright file="EngineTimer.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Core
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The Engine  Timer provides a means to start a timer with a callback that can be repeated over a set interval.
    /// </summary>
    /// <typeparam name="T">A generic Type</typeparam>
    public sealed class EngineTimer<T> : CancellationTokenSource, IDisposable
    {
        /// <summary>
        /// The timer task
        /// </summary>
        private Task timerTask;

        /// <summary>
        /// The callback
        /// </summary>
        private Action<T, EngineTimer<T>> callback;

        /// <summary>
        /// How many times we have fired the timer thus far.
        /// </summary>
        private long fireCount = 0;

        /// <summary>
        /// The number of times that the timer can be fired before it is stopped.
        /// If set to 0, then it will fire forever unless manually stopped.
        /// </summary>
        private int numberOfFires = 0;

        /// <summary>
        /// The interval at which the timer is fired.
        /// </summary>
        private double interval = 0.0;
        
        /// <summary>
        /// Dictates whether we invoke the timer callback on a worker thread or not.
        /// </summary>
        private bool callbackOnWorkerThread = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineTimer{T}"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        public EngineTimer(Action<T, EngineTimer<T>> callback, T state)
        {
            this.callback = callback;
            this.StateData = state;
        }

        /// <summary>
        /// Gets the state data.
        /// </summary>
        /// <value>
        /// The state data.
        /// </value>
        public T StateData { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the engine timer is currently running.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Starts the specified start delay.
        /// </summary>
        /// <param name="startDelay">The start delay in milliseconds.</param>
        /// <param name="interval">The interval in milliseconds.</param>
        /// <param name="isOneShot">if set to <c>true</c> [is one shot].</param>
        /// <param name="callbackOnWorkerThread">if set to <c>true</c> the callback will be performed on a background thread.</param>
        public void Start(double startDelay, double interval, bool isOneShot, bool callbackOnWorkerThread = false)
        {
            this.IsRunning = true;
            this.interval = interval;
            this.numberOfFires = isOneShot ? 1 : 0;
            this.callbackOnWorkerThread = callbackOnWorkerThread;

            this.timerTask = Task<T>
                .Delay(TimeSpan.FromMilliseconds(startDelay), this.Token)
                .ContinueWith(
                    FireTimer,
                    Tuple.Create(this.callback, this.StateData),
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.Default);
        }

        /// <summary>
        /// Starts the specified start delay.
        /// </summary>
        /// <param name="startDelay">The start delay in milliseconds.</param>
        /// <param name="interval">The interval in milliseconds.</param>
        /// <param name="numberOfFires">Specifies the number of times to invoke the timer callback when the interval is reached. Set to 0 for infinite.</param>
        /// <param name="callbackOnWorkerThread">if set to <c>true</c> the callback will be performed on a background thread.</param>
        public void Start(double startDelay, double interval, int numberOfFires, bool callbackOnWorkerThread = false)
        {
            this.IsRunning = true;
            this.interval = interval;
            this.numberOfFires = numberOfFires;
            this.callbackOnWorkerThread = callbackOnWorkerThread;

            this.timerTask = Task
                .Delay(TimeSpan.FromMilliseconds(startDelay), this.Token)
                .ContinueWith(
                    FireTimer,
                    Tuple.Create(this.callback, this.StateData),
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.Default);
        }

        private async Task FireTimer(Task task, object state)
        {
            Debug.WriteLine(string.Format(
                "Starting engine timer for {0} with an interval of {1}ms",
                typeof(T).Name,
                this.interval));

            var tuple = (Tuple<Action<T, EngineTimer<T>>, T>)state;
            while (!this.IsCancellationRequested)
            {
                // Only increment if we are supposed to.
                if (this.numberOfFires > 0)
                {
                    this.fireCount++;
                }

                this.ExecuteCallback(tuple, task);

                // If we have reached our fire count, stop. If set to 0 then we fire until manually stopped.
                if (this.numberOfFires > 0 || this.fireCount >= this.numberOfFires)
                {
                    this.Stop();
                }

                await Task.Delay(TimeSpan.FromMilliseconds(this.interval), this.Token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Stops the timer for this instance. It is not 
        /// </summary>
        public void Stop()
        {
            if (!this.IsCancellationRequested)
            {
                this.Cancel();
            } 
            this.IsRunning = false;
        }

        /// <summary>
        /// Cancels the timer and releases the unmanaged resources used by the <see cref="T:System.Threading.CancellationTokenSource" /> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.IsRunning = false;
                this.Cancel();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Executes the callback.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="owningTask">The owning task.</param>
        /// <param name="callbackOnWorkerThread">if set to <c>true</c> [callback on worker thread].</param>
        private void ExecuteCallback(Tuple<Action<T, EngineTimer<T>>, T> state, Task owningTask)
        {
            if (this.callbackOnWorkerThread)
            {
                Task.Run(() =>
                {
                    Debug.WriteLine(string.Format(
                        "Updating engine timer for {0} with Task Id: {1}",
                        typeof(T).Name,
                        owningTask.Id));
                    state.Item1(state.Item2, this);
                });
            }
            else
            {
                state.Item1(state.Item2, this);
            }
        }
    }
}
