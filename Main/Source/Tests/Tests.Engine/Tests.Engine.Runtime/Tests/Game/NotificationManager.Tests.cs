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
            var notificationCenter = new NotificationManager();
            var notification = new TestNotificationFixture<MessageFixture>(notificationCenter);
            bool callbackCalled = false;

            notification.Register((message, subscription) => callbackCalled = true);
            notificationCenter.Subscribe<MessageFixture>(notification);

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
            var notification = new TestNotificationFixture<MessageFixture>(notificationCenter);
            notification.Register((message, sub) => callCount++);

            // Subscribe our notification and publish a new message
            notificationCenter.Subscribe(notification);
            notificationCenter.Publish(new MessageFixture("Test"));

            // Act
            // Unsubscribe the notification and attempt a new publish
            notificationCenter.Unsubscribe<MessageFixture>(notification);
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
            var messageNotification = new TestNotificationFixture<MessageFixture>(notificationCenter);
            var secondaryNotification
                = new TestNotificationFixture<SecondaryNotificationFixture>(notificationCenter);

            messageNotification.Register(
                (message, sub) => 
                    ExceptionFactory.ThrowIf<InvalidOperationException>(message.GetType() != typeof(MessageFixture)));
            secondaryNotification.Register(
                (message, sub) => 
                    ExceptionFactory.ThrowIf<InvalidOperationException>(message.GetType() != typeof(SecondaryNotificationFixture)));

            // Subscribe
            notificationCenter.Subscribe(messageNotification);
            notificationCenter.Subscribe(secondaryNotification);

            // Act
            notificationCenter.Publish(new MessageFixture("Test"));
            notificationCenter.Publish(new SecondaryNotificationFixture(new GameComponentFixture()));
        }
    }
}
