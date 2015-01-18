using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using Moq;
using Mud.Engine.Runtime.Game;
using Mud.Engine.Runtime.Game.Environment;
using Mud.Engine.Runtime.Services;

namespace Tests.Engine.Runtime.Game
{
    [TestClass]
    public class DefaultGameTests
    {
        private IContainer container;

        [TestInitialize]
        public void Setup()
        {
            var builder = new ContainerBuilder();

            // Set up our mock Log and register it to the container
            var loggerMock = new Mock<ILoggingService>();
            builder.RegisterInstance(loggerMock.Object).As<ILoggingService>();

            // Set up the worlds service mock and register it to the container
            var worldServiceMock = new Mock<IWorldService>();
            worldServiceMock
                .Setup(service => service.GetAllWorlds())
                .ReturnsAsync(new List<DefaultWorld>()
                {
                    new DefaultWorld
                    {
                        TimeOfDayStates = new List<TimeOfDayState> { new TimeOfDayState() }
                    }
                });
            builder.RegisterInstance(worldServiceMock.Object).As<IWorldService>();

            // Build the IoC container.
            container = builder.Build();
        }

        [TestMethod]
        [TestCategory("Runtime - DefaultGame")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_world_service_throws_exception()
        {
            // Arrange
            var game = new DefaultGame(container.Resolve<ILoggingService>(), null);

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [TestCategory("Runtime - DefaultGame")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_logging_service_throws_exception()
        {
            // Arrange
            var game = new DefaultGame(null, container.Resolve<IWorldService>());

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        [TestCategory("Runtime - DefaultGame")]
        public async Task Initialize_loads_worlds()
        {
            // Arrange
            var game = new DefaultGame(container.Resolve<ILoggingService>(), container.Resolve<IWorldService>());

            // Act
            await game.Initialize();

            // Assert
            Assert.IsNotNull(game.Worlds);
            Assert.IsTrue(game.Worlds.Count == 1);
        }

        [TestMethod]
        [TestCategory("Runtime - DefaultGame")]
        public async Task Initializes_loaded_worlds()
        {
            // Arrange
            var world = new DefaultWorld();
            bool worldLoaded = false;
            world.Loaded += (sender, args) => worldLoaded = true;
            world.TimeOfDayStates = new List<TimeOfDayState> { new TimeOfDayState() };

            // Set up the world service to return our mocked World.
            var worldServiceMock = new Mock<IWorldService>();
            worldServiceMock
                .Setup(service => service.GetAllWorlds())
                .ReturnsAsync(new List<DefaultWorld> { world });
            var game = new DefaultGame(container.Resolve<ILoggingService>(), worldServiceMock.Object);

            // Act
            await game.Initialize();

            // Assert
            Assert.IsTrue(worldLoaded);
        }

        [TestMethod]
        [TestCategory("Runtime - DefaultGame")]
        public async Task Initialize_with_autosave()
        {
            // Arrange
            var game = new DefaultGame(container.Resolve<ILoggingService>(), container.Resolve<IWorldService>());
            game.Autosave.AutoSaveFrequency = 5;

            // Act
            await game.Initialize();

            // Assert
            Assert.IsTrue(game.Autosave.IsAutosaveRunning);
        }

        [TestMethod]
        [TestCategory("Runtime - DefaultGame")]
        public async Task Initialize_without_autosave()
        {
            // Arrange
            var game = new DefaultGame(container.Resolve<ILoggingService>(), container.Resolve<IWorldService>());
            game.Autosave.AutoSaveFrequency = 0;

            // Act
            await game.Initialize();

            // Assert
            Assert.IsFalse(game.Autosave.IsAutosaveRunning);
        }

        [TestMethod]
        [TestCategory("Runtime - DefaultGame")]
        public async Task World_loaded_event_fired()
        {
            // Arrange
            var game = new DefaultGame(container.Resolve<ILoggingService>(), container.Resolve<IWorldService>());
            bool worldProvided = false;
            game.WorldLoaded += (sender, args) =>
            {
                worldProvided = args.World != null;
                return Task.FromResult(true);
            };

            // Act
            await game.Initialize();

            // Assert
            Assert.IsTrue(worldProvided, "World not provided.");
        }

        [TestMethod]
        [TestCategory("Runtime - DefaultGame")]
        public async Task Delete_clears_worlds()
        {
            // Arrange
            // A DefaultWorld mock is needed so we can verify DefaultWorld.Delete is called.
            var worldMock = new Mock<DefaultWorld>();

            // Get our mocked instance and set up the time of day states needed
            // by the Initialize method call.
            var world = worldMock.Object;
            world.TimeOfDayStates = new List<TimeOfDayState> { new TimeOfDayState() };

            // Set up the world service to return our mocked World
            // and allow us to verify that save was called.
            var worldServiceMock = new Mock<IWorldService>();
            worldServiceMock
                .Setup(service => service.GetAllWorlds())
                .ReturnsAsync(new List<DefaultWorld> { worldMock.Object });
            worldServiceMock
                .Setup(service => service.SaveWorld(It.IsAny<DefaultWorld>()))
                .Returns(Task.FromResult(0))
                .Verifiable();
            var game = new DefaultGame(container.Resolve<ILoggingService>(), worldServiceMock.Object);
            await game.Initialize();

            // Act
            await game.Delete();

            // Assert
            Assert.AreEqual(0, game.Worlds.Count, "Worlds not cleared.");
            worldServiceMock.Verify(service => service.SaveWorld(It.IsAny<DefaultWorld>()));

            // TODO: Verify DefaultWorld.Delete is called.
            //worldMock.Verify(world => world.Delete());
        }

        [TestMethod]
        [TestCategory("Runtime - DefaultGame")]
        public async Task Delete_stops_autosaver()
        {
            // Arrange
            var game = new DefaultGame(container.Resolve<ILoggingService>(), container.Resolve<IWorldService>());
            await game.Initialize();

            // Act
            await game.Delete();

            // Assert
            Assert.IsFalse(game.Autosave.IsAutosaveRunning, "Autosave is still running.");
        }
    }
}
