using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Diagnostics;
using Mud.Engine.Runtime;

namespace Tests.Engine.Runtime
{
    [TestClass]
    public class EngineTimerTests
    {
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
