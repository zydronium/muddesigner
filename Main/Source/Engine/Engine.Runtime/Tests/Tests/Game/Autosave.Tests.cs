using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mud.Engine.Runtime.Game;
using System.Threading.Tasks;

namespace Tests.Engine.Runtime.Game
{
    /// <summary>
    /// Tests that Autosave class in the engine runtime to ensure it attempts to
    /// fire its autosave callback correctly.
    /// </summary>
    [TestClass]
    public class AutosaveTests
    {
        /// <summary>
        /// Tests that a null callback throws an ArgumentNullException
        /// </summary>
        [TestMethod]
        [TestCategory("Runtime.Game - Autosave")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_callback_throws_exception()
        {
            // Act
            var autosave = new Autosave<object>(new object(), null);

            // Assert
            Assert.Fail();
        }
        /// <summary>
        /// Tests that a null item to save throws an ArgumentNullException
        /// </summary>
        [TestMethod]
        [TestCategory("Runtime.Game - Autosave")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_item_throws_exception()
        {
            // Act
            var autosave = new Autosave<object>(null, () => Task.FromResult(true));

            // Assert
            Assert.Fail();
        }

        /// <summary>
        /// Tests that enabling the Autosave frequency allows for the callback
        /// to get invoked
        /// </summary>
        [TestMethod]
        [TestCategory("Runtime.Game - Autosave")]
        public async Task Frequency_enables_autosave()
        {
            // Arrange
            var autosave = new Autosave<object>(new object(), () => Task.FromResult(true));

            // We don't test that the callback happens at this frequency; 
            // that is covered by the EngineTimer tests.
            autosave.AutoSaveFrequency = 1;

            // Act
            await autosave.Initialize();

            // Assert
            Assert.IsTrue(autosave.IsAutosaveRunning, "Autosave is not running");
        }

        /// <summary>
        /// Tests that not having a autosave frequency value assigned results in
        /// the autosave not firing.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestCategory("Runtime.Game - Autosave")]
        public async Task Frequency_disables_autosave()
        {
            // Arrange
            var autosave = new Autosave<object>(new object(), () => Task.FromResult(true));
            autosave.AutoSaveFrequency = 0;

            // Act
            await autosave.Initialize();

            // Assert
            Assert.IsFalse(autosave.IsAutosaveRunning, "Autosave is running");
        }

        /// <summary>
        /// Tests that deleting the autosave component results in the autosave
        /// timer stopping.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestCategory("Runtime.Game - Autosave")]
        public async Task Delete_stops_autosaving()
        {
            // Arrange
            var autosave = new Autosave<object>(new object(), () => Task.FromResult(true));
            autosave.AutoSaveFrequency = 1;
            await autosave.Initialize();

            // Act
            await autosave.Delete();

            // Assert
            Assert.IsFalse(autosave.IsAutosaveRunning, "Autosave is running");
        }
    }
}
