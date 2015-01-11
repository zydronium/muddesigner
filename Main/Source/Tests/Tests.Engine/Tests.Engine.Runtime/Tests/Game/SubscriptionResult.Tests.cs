using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mud.Engine.Runtime.Game.Character;
using Mud.Engine.Runtime.Game;

namespace Tests.Engine.Runtime.Game
{
    [TestClass]
    public class SubscriptionResultTests
    {
        private Mock<ICharacter> characterMock = new Mock<ICharacter>();

        [TestMethod]
        public void Conditions_for_publish_are_met_and_dispatched()
        {
            // Arrange
            string badMessage = "Bad Message";
            string expectedMessage = "expected.";
            string receivedMessage = string.Empty;

            // Subscribe
            NotificationManager.CurrentCenter
                .Subscribe<WhisperMessage>()
                .If(msg => !string.IsNullOrWhiteSpace(msg.Message))
                .If(msg => !msg.Message.Equals(badMessage))
                .Register((msg, subscription) => receivedMessage = msg.Message);

            // Act
            NotificationManager.CurrentCenter
                .Publish(new WhisperMessage(expectedMessage, characterMock.Object));

            // Assert
            Assert.AreEqual(expectedMessage, receivedMessage);
        }

        [TestMethod]
        public void Object_can_unsubscribe()
        {
            // Arrange
            string receivedMessage = string.Empty;
            ISubscription handler = NotificationManager.CurrentCenter.Subscribe<WhisperMessage>()
                .Register((msg, subscription) => receivedMessage = msg.Message);

            // Act
            handler.Unsubscribe();
            NotificationManager.CurrentCenter
                .Publish(new WhisperMessage("Some message", characterMock.Object));

            // Assert
            Assert.AreEqual(string.Empty, receivedMessage);
        }

        [TestMethod]
        public void Object_can_unsubscribe_within_callback()
        {
            // Arrange
            string receivedMessage = string.Empty;
            string expectedMessage = "Some Message";
            ISubscription handler = NotificationManager.CurrentCenter.Subscribe<WhisperMessage>()
                .Register((msg, subscription) =>
                    {
                        receivedMessage = msg.Message;
                        subscription.Unsubscribe();
                    });

            // Act
            NotificationManager.CurrentCenter
                .Publish(new WhisperMessage(expectedMessage, characterMock.Object));
            NotificationManager.CurrentCenter
                .Publish(new WhisperMessage("Attempted to change message", characterMock.Object));

            // Assert
            Assert.AreEqual(expectedMessage, receivedMessage);
        }
    }
}
