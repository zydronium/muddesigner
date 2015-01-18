using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mud.Engine.Runtime.Game;
using Tests.Engine.Runtime.Fixtures;
using Mud.Engine.Runtime;

namespace Tests.Engine.Runtime.Tests.Game
{
    [TestClass]
    public class NotificationManagerTests
    {
        [TestMethod]
        [TestCategory("Runtime.Game - NotificationManager")]
        public void Publish_invokes_callbacks()
        {
            bool callbackCalled = false;
            var notificationCenter = new NotificationManager();
            notificationCenter.Subscribe<MessageFixture>((msg, sub) => callbackCalled = true);

            // Act
            notificationCenter.Publish(new MessageFixture("Test"));

            // Assert
            Assert.IsTrue(callbackCalled);
        }

        [TestMethod]
        [TestCategory("Runtime.Game - NotificationManager")]
        public void Handler_can_unsubscribe()
        {
            var notificationCenter = new NotificationManager();
            int callCount = 0;
            
            // Build our notification.
            ISubscription subscriber = notificationCenter.Subscribe<MessageFixture>(
                (message, sub) => callCount++);

            // Subscribe our notification and publish a new message
            notificationCenter.Publish(new MessageFixture("Test"));

            // Act
            // Unsubscribe the notification and attempt a new publish
            subscriber.Unsubscribe();
            notificationCenter.Publish(new MessageFixture("Test"));

            // Assert
            Assert.AreEqual(1, callCount, "The callbacks were not fired properly");
        }

        [TestMethod]
        [TestCategory("Runtime.Game - NotificationManager")]
        public void Handler_receives_only_its_message()
        {
            // Set up the first handler
            var notificationCenter = new NotificationManager();
            notificationCenter.Subscribe<MessageFixture>(
                (message, sub) => 
                    ExceptionFactory.ThrowIf<InvalidOperationException>(message.GetType() != typeof(MessageFixture)));
            notificationCenter.Subscribe<SecondaryMessageFixture>(
                (message, sub) => 
                    ExceptionFactory.ThrowIf<InvalidOperationException>(message.GetType() != typeof(SecondaryMessageFixture)));

            // Act
            notificationCenter.Publish(new MessageFixture("Test"));
            notificationCenter.Publish(new SecondaryMessageFixture(new GameComponentFixture()));
        }
    }
}
