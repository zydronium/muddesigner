using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Engine.Runtime.Fixtures;
using Mud.Engine.Runtime;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Tests.Engine.Runtime.Tests.Game
{
    /// <summary>
    /// Unit Tests testing that the GameComponent Type invokes its events properly.
    /// </summary>
    [TestClass]
    public class GameComponentTests
    {
        private GameComponentFixture componentFixture = new GameComponentFixture();

        /// <summary>
        /// Ensures that the Loading event is raised when Initialize is called.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestCategory("Runtime.Game - GameComponent")]
        public async Task Initialize_raises_loading_began_event()
        {
            // Arrange
            bool loading = false;
            componentFixture.Loading += async (component) => await TaskFromAction.Create(() => loading = true);
            componentFixture.LoadDelegate = () => Task.FromResult(true);

            // Arrange
            await this.componentFixture.Initialize();

            // Assert
            Assert.IsTrue(loading);
        }

        /// <summary>
        /// Ensures that the Load protected method is invoked when Initialize is called.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestCategory("Runtime.Game - GameComponent")]
        public async Task Initialize_invokes_load_method()
        {
            // Arrange
            bool loading = false;
            componentFixture.LoadDelegate = () => TaskFromAction.Create(() => loading = true);

            // Arrange
            await this.componentFixture.Initialize();

            // Assert
            Assert.IsTrue(loading);
        }

        /// <summary>
        /// Ensures that the Loaded event handler is raised
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestCategory("Runtime.Game - GameComponent")]
        public async Task Initialize_raises_load_completed_event()
        {
            // Arrange
            bool loading = false;
            componentFixture.Loaded += (component, args) => loading = true;
            componentFixture.LoadDelegate = () => Task.FromResult(true);

            // Arrange
            await this.componentFixture.Initialize();

            // Assert
            Assert.IsTrue(loading);
        }

        /// <summary>
        /// Ensures that the Loading, Load and Loaded methods are invoked in the correct order.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestCategory("Runtime.Game - GameComponent")]
        public async Task Initialize_order_of_operations_are_correct()
        {
            // Arrange
            const string loadingOperation = "Loading";
            const string loadOperation = "Load";
            const string loadedOPeration = "Loaded";
            var operationResults = new List<Tuple<string, bool>>();

            componentFixture.Loading += 
                async (component) => await TaskFromAction.Create(
                    () => operationResults.Add(new Tuple<string, bool>(loadingOperation, true))); 
            componentFixture.Loaded += 
                (component, args) => operationResults.Add(new Tuple<string, bool>(loadedOPeration, true));
            componentFixture.LoadDelegate = () => TaskFromAction.Create(
                () => operationResults.Add(new Tuple<string, bool>(loadOperation, true)));

            // Arrange
            await this.componentFixture.Initialize();

            // Assert
            Assert.IsTrue(operationResults[0].Item1 == loadingOperation && operationResults[0].Item2, "Loading was not invoked first.");
            Assert.IsTrue(operationResults[1].Item1 == loadOperation && operationResults[1].Item2, "Load was not invoked 2nd.");
            Assert.IsTrue(operationResults[2].Item1 == loadedOPeration && operationResults[2].Item2, "Loaded was not invoked last.");
        }

        /// <summary>
        /// Ensures that the Deleting event is raised when Delete is called.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestCategory("Runtime.Game - GameComponent")]
        public async Task Delete_raises_delete_began_event()
        {
            // Arrange
            bool deleting = false;
            componentFixture.Deleting += async (component) => await TaskFromAction.Create(() => deleting = true);
            componentFixture.UnloadDelegate = () => Task.FromResult(true);

            // Arrange
            await this.componentFixture.Delete();

            // Assert
            Assert.IsTrue(deleting);
        }

        /// <summary>
        /// Ensures that the Unload protected method is invoked when Delete is called.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestCategory("Runtime.Game - GameComponent")]
        public async Task Delete_invokes_unload_method()
        {
            // Arrange
            bool deleting = false;
            componentFixture.UnloadDelegate = () => TaskFromAction.Create(() => deleting = true);

            // Arrange
            await this.componentFixture.Delete();

            // Assert
            Assert.IsTrue(deleting);
        }

        /// <summary>
        /// Ensures that the Deleted event handler is raised
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestCategory("Runtime.Game - GameComponent")]
        public async Task Delete_raises_delete_completed_event()
        {
            // Arrange
            bool deleted = false;
            componentFixture.Deleted += (component, args) => deleted = true;
            componentFixture.UnloadDelegate = () => Task.FromResult(true);

            // Arrange
            await this.componentFixture.Delete();

            // Assert
            Assert.IsTrue(deleted);
        }

        /// <summary>
        /// Ensures that the Deleting, Unload and Deleted methods are invoked in the correct order.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestCategory("Runtime.Game - GameComponent")]
        public async Task Delete_order_of_operations_are_correct()
        {
            // Arrange
            const string deletingOperation = "Deleting";
            const string unloadOperation = "Unload";
            const string deletedOperation = "Deleted";
            var operationResults = new List<Tuple<string, bool>>();

            componentFixture.Deleting +=
                async (component) => await TaskFromAction.Create(
                    () => operationResults.Add(new Tuple<string, bool>(deletingOperation, true)));
            componentFixture.Deleted +=
                (component, args) => operationResults.Add(new Tuple<string, bool>(deletedOperation, true));
            componentFixture.UnloadDelegate = () => TaskFromAction.Create(
                () => operationResults.Add(new Tuple<string, bool>(unloadOperation, true)));

            // Arrange
            await this.componentFixture.Delete();

            // Assert
            Assert.IsTrue(operationResults[0].Item1 == deletingOperation && operationResults[0].Item2, "Loading was not invoked first.");
            Assert.IsTrue(operationResults[1].Item1 == unloadOperation && operationResults[1].Item2, "Load was not invoked 2nd.");
            Assert.IsTrue(operationResults[2].Item1 == deletedOperation && operationResults[2].Item2, "Loaded was not invoked last.");
        }
    }
}
