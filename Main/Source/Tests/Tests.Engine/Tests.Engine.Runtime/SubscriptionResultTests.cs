using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mud.Engine.Runtime;

namespace Tests.Engine.Runtime
{
    [TestClass]
    public class SubscriptionResultTests
    {
        [TestMethod]
        public void Conditions_for_publish_are_met_and_dispatched()
        {
            // Arrange
            string badMessage = "Bad Message";
            string expectedMessage = "expected.";
            string receivedMessage = string.Empty;

            // Subscribe
            NotificationManager.CurrentCenter
                .Subscribe<ChatMessage>()
                .If(msg => !string.IsNullOrWhiteSpace(msg.Message))
                .If(msg => !msg.Message.Equals(badMessage))
                .Register(msg => receivedMessage = msg.Message);

            // Act
            NotificationManager.CurrentCenter.Publish(new ChatMessage(expectedMessage));

            // Assert
            Assert.AreEqual(expectedMessage, receivedMessage);
        }

        [TestMethod]
        public void Object_can_unsubscribe()
        {
            // Arrange
            string receivedMessage = string.Empty;
            ISubscription handler = NotificationManager.CurrentCenter.Subscribe<ChatMessage>()
                .Register(msg => receivedMessage = msg.Message);

            // Act
            handler.Unsubscribe();
            NotificationManager.CurrentCenter.Publish(new ChatMessage("Some message"));

            // Assert
            Assert.AreEqual(string.Empty, receivedMessage);
        }
    }
}
