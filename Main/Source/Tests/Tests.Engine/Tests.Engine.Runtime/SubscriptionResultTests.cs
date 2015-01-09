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
            var center = ChatCenter.CurrentCenter;

            // Subscribe
            center.Subscribe<ChatMessage>()
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
            center.Publish(new ChatMessage(string.Empty));

            // Assert
            Assert.AreEqual(3, x);
            Assert.AreEqual(10, y);
            Assert.AreEqual(15, z);
        }

        [TestMethod]
        public void Object_can_unsubscribe()
        {
            // Arrange
            var chatHandler = new ChatMessageHandler();
            int y = 2;
            var callback = new Action<IMessage>(msg => y = 10);

            // Subscribe
            ISubscriptionHandler handler = ChatCenter.CurrentCenter.Subscribe<ChatMessage>()
                .Dispatch(callback);
            handler.Unsubscribe();

            // Act
            ChatCenter.CurrentCenter.Publish(new ChatMessage(string.Empty));

            // Assert
            Assert.AreEqual(2, y);
        }
    }
}
