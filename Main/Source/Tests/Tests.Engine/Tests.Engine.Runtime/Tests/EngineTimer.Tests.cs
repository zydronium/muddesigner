using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Diagnostics;
using Mud.Engine.Runtime;

namespace Tests.Engine.Runtime
{
    /// <summary>
    /// Unit Tests for the EngineTimer class.
    /// </summary>
    [TestClass]
    public class EngineTimerTests
    {
        /// <summary>
        /// Ensures that the timer fires at the correct interval.
        /// 
        /// Since there is some time lost due to having to invoke the callback
        /// and actually get to the Stop() method, we Assert if we are within a 
        /// set range of milliseconds. We will never hit the target time exactly.
        /// </summary>
        [TestMethod]
        [TestCategory("Runtime - EngineTimer")]
        public void Engine_timer_fires_at_proper_interval()
        {
            // Arrange
            int callbackCount = 0;
            int targetMilliseconds = 1000;
            DateTime initialTime;
            DateTime callbackTimeStamp = DateTime.Now;
            var engineTimer = new EngineTimer<object>(new object());

            // Act
            initialTime = DateTime.Now;
            engineTimer.Start(0, targetMilliseconds, 0, (message, timer) =>
            {
                // Skip the first interval, since it is done immediately.
                if (callbackCount == 1)
                {
                    callbackTimeStamp = DateTime.Now;
                    timer.Stop();
                }
                else
                {
                    callbackCount++;
                    initialTime = DateTime.Now;
                }
            });
            while (engineTimer.IsRunning) { Thread.Sleep(1); }

            // Assert
            TimeSpan difference = callbackTimeStamp.Subtract(initialTime);
            Debug.WriteLine(string.Format("Callback time was {0} milliseconds", difference.TotalMilliseconds));

            // We allow a variance of 20ms from the target time since we loose time from when we capture the initial date and fire the actual interval.
            Assert.IsTrue(difference.TotalMilliseconds < (targetMilliseconds + 20) && difference.TotalMilliseconds > (targetMilliseconds - 20));
        }

        /// <summary>
        /// Ensures the EngineTimer fires after a set delay.
        /// </summary>
        [TestMethod]
        [TestCategory("Runtime - EngineTimer")]
        public void Engine_timer_fires_with_delay_time()
        {
            // Arrange
            int targetMilliseconds = 500; 
            DateTime initialTime;
            DateTime callbackTimeStamp = DateTime.Now;
            var engineTimer = new EngineTimer<object>(new object());

            // Act
            initialTime = DateTime.Now;
            engineTimer.Start(targetMilliseconds, 0, 1, (message, timer) =>
            {
                // Skip the first interval, since it is done immediately.
                callbackTimeStamp = DateTime.Now;
                timer.Stop();
            });
            while (engineTimer.IsRunning) { }

            // Assert
            TimeSpan difference = callbackTimeStamp.Subtract(initialTime);

            // We allow a variance of 20ms from the target time since we loose time from when we capture the initial date and fire the actual interval.
            Assert.IsTrue(difference.TotalMilliseconds < (targetMilliseconds + 20) && difference.TotalMilliseconds > (targetMilliseconds - 20));
        }
    }
}
