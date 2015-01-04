using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Autofac;
using System.Threading.Tasks;
using Mud.Engine.Runtime;
using Mud.Engine.Runtime.Game;
using Mud.Engine.Runtime.Services;

namespace Tests.Engine.Runtime.Core
{
    [TestClass]
    public class DefaultGameTests
    {
        private IContainer container;

        [TestInitialize]
        public void Setup()
        {
            var builder = new ContainerBuilder();

            // Set up our mock Log and World services.
            var loggerMock = new Mock<ILoggingService>();

            // Register our types.
            builder.RegisterInstance(loggerMock.Object).As<ILoggingService>();

            // Build the IoC container.
            container = builder.Build();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Game_throws_exception_with_invalid_world_service()
        {
            // Arrange
            var game = new DefaultGame(container.Resolve<ILoggingService>(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Game_throws_exception_with_invalid_logging_service()
        {
            // Arrange
            var game = new DefaultGame(null, container.Resolve<IWorldService>());
        }

        [TestMethod]
        public async Task Game_can_initialize()
        {
            // Arrange
            var game = new DefaultGame(container.Resolve<ILoggingService>(), container.Resolve<IWorldService>());

            // Act
            await game.Initialize();

            // Assert
            Assert.IsNotNull(game.Worlds);
            Assert.IsTrue(game.Worlds.Count == 1);
        }
    }
}
