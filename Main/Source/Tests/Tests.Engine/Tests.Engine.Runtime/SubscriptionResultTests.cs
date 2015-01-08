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
            var chatHandler = new ChatMessageHandler();
            int x = 1;
            int y = 2;
            int z = 3;

            // Subscribe
            MessageCenter.Subscribe<ChatMessage>()
                .If(msg => x == 1)
                .If(msg => y == 2)
                .If(msg => z == 3)
                .Dispatch(msg => x = 3)
                .Dispatch(msg =>
                {
                    y = 10;
                    z = 15;
                });

            // Act
            MessageCenter.Publish(new ChatMessage(string.Empty));

            // Assert
            Assert.AreEqual(3, x);
            Assert.AreEqual(10, y);
            Assert.AreEqual(15, z);
        }

        [TestMethod]
        public void Chat_message_is_delivered()
        {
            var chatHandler = new ChatMessageHandler();
            MessageCenter.Subscribe<ChatMessage>();
        }
    }
}
