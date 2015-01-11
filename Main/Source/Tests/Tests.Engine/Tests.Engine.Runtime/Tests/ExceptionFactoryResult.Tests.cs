using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mud.Engine.Runtime;

namespace Tests.Engine.Runtime
{
    [TestClass]
    public class ExceptionFactoryResultTests
    {
        [TestMethod]
        [TestCategory("Runtime - ExceptionFactoryResult")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_callback_for_elsedo_throws_exception()
        {
            // Arrange
            var result = new ExceptionFactoryResult<ArgumentNullException>(null);

            // Act
            result.ElseDo(null);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactoryResult")]
        public void ElseDo_callback_is_invoked()
        {
            // Arrange
            bool callbackInvoked = false;
            Action callback = () => callbackInvoked = true;
            var result = new ExceptionFactoryResult<ArgumentNullException>(null);

            // Act
            result.ElseDo(callback);

            // Assert
            Assert.IsTrue(callbackInvoked);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Or_predicate_is_true_with_default_factory()
        {
            // Arrange
            object obj = null;

            // Act
            new ExceptionFactoryResult<ArgumentNullException>(null).Or(() => obj == null);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        public void Or_predicate_is_false_with_default_factory()
        {
            // Arrange
            object obj = null;

            // Act
            new ExceptionFactoryResult<ArgumentNullException>(null).Or(() => obj != null);

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Or_predicate_is_true_with_custom_factory()
        {
            // Arrange
            object obj = null;

            // Act
            new ExceptionFactoryResult<ArgumentNullException>(null).Or(
                () => obj == null,
                () => new InvalidOperationException());
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        public void Or_predicate_is_false_with_custom_factory()
        {
            // Arrange
            object obj = null;

            // Act
            new ExceptionFactoryResult<ArgumentNullException>(null).Or(
                () => obj != null,
                () => new NullReferenceException());

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Or_condition_is_true_with_default_factory()
        {
            // Act
            new ExceptionFactoryResult<ArgumentNullException>(null).Or(true);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        public void Or_condition_is_false_with_default_factory()
        {
            // Act
            new ExceptionFactoryResult<ArgumentNullException>(null).Or(false);

            // Assert
            Assert.IsTrue(true);
        }
    }
}
