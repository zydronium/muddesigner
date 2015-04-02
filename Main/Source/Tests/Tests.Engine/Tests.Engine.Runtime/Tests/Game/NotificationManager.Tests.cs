using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mud.Engine.Runtime.Game;
using Tests.Engine.Runtime.Fixtures;
using Mud.Engine.Runtime;
using Mud.Engine.Runtime.Game.Character;

namespace Tests.Engine.Runtime.Tests.Game
{
    [TestClass]
    public class NotificationManagerTests
    {
        [TestMethod]
        [TestCategory("Runtime.Game - NotificationManager")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Subscribe_with_null_calback_throws_exception()
        {
            // Arrange
            var notificationCenter = new NotificationManager();

            // Act
            notificationCenter.Subscribe<ShoutMessage>(null);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [TestCategory("Runtime.Game - NotificationManager")]
        public void Publish_invokes_callbacks()
        {
            bool callbackCalled = false;
            string messageContent = "Test";
            var notificationCenter = new NotificationManager();
            notificationCenter.Subscribe<ShoutMessage>((msg, sub) =>
                {
                    if (msg.Content == messageContent)
                    {
                        callbackCalled = true;
                    }
                });

            // Act
            //notificationCenter.Publish(new ShoutMessage(
            //    messageContent,
            //    new DefaultPlayer(null, notificationCenter)));  

            // Assert
            Assert.IsTrue(callbackCalled);
        }

        [TestMethod]
        [TestCategory("Runtime.Game - NotificationManager")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Publish_with_null_message_throws_exception()
        {
            var notificationCenter = new NotificationManager();
            notificationCenter.Subscribe<ShoutMessage>((msg, sub) => { });

            // Act
            notificationCenter.Publish<ShoutMessage>(null);

            // Assert
            Assert.Fail();
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
            // Arrange
            // Set up the first handler
            var notificationCenter = new NotificationManager();

            notificationCenter.Subscribe<MessageFixture>(
                (message, sub) => ExceptionFactory
                    .ThrowIf<InvalidOperationException>(message.GetType() != typeof(MessageFixture)));

            notificationCenter.Subscribe<SecondaryMessageFixture>(
                (message, sub) => ExceptionFactory
                        .ThrowIf<InvalidOperationException>(message.Content.GetType() != typeof(DefaultPlayer)));

            // Act
            notificationCenter.Publish(new MessageFixture("Test"));
            //notificationCenter.Publish(new SecondaryMessageFixture(new DefaultPlayer(null, notificationCenter)));
        }
    }
}
