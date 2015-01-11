using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mud.Engine.Runtime;
using Moq;
using System.Collections.Generic;
using Mud.Engine.Runtime.Game;

namespace Tests.Engine.Runtime
{
    [TestClass]
    public class ExceptionFactoryTests
    {
        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_predicate_throws_exception()
        {
            // Arrange
            object obj = null;

            // Act
            ExceptionFactory.ThrowIf<NullReferenceException>(null);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Null_factory_throws_exception()
        {
            // Arrange
            object obj = null;

            // Act
            ExceptionFactory.ThrowIf<NullReferenceException>(obj == null, () => null);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        [ExpectedException(typeof(NullReferenceException))]
        public void Predicate_is_true_with_default_factory()
        {
            // Arrange
            object obj = null;

            // Act
            ExceptionFactory.ThrowIf<NullReferenceException>(() => obj == null);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        public void Predicate_is_false_with_default_factory()
        {
            // Arrange
            object obj = null;

            // Act
            ExceptionFactory.ThrowIf<NullReferenceException>(() => obj != null);

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        [ExpectedException(typeof(NullReferenceException))]
        public void Predicate_is_true_with_custom_exception_factory()
        {
            //Arrange
            object obj = null;

            // Act
            ExceptionFactory.ThrowIf(
                () => obj == null,
                () => new NullReferenceException());
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        public void Predicate_is_false_with_custom_exception_factory()
        {
            // Act
            object obj = null;

            // Arrange
            ExceptionFactory.ThrowIf(
                () => obj != null,
                () => new NullReferenceException());

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        [ExpectedException(typeof(NullReferenceException))]
        public void Condition_is_true_with_default_exception_factory()
        {
            // Act
            ExceptionFactory.ThrowIf<NullReferenceException>(true);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        public void Condition_is_false_with_default_exception_factory()
        {
            // Act
            ExceptionFactory.ThrowIf<NullReferenceException>(false);

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        [ExpectedException(typeof(NullReferenceException))]
        public void Condition_is_true_with_custom_exception_factory()
        {
            ExceptionFactory.ThrowIf(
                true,
                () => new NullReferenceException());
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        public void Condition_is_false_with_custom_exception_factory()
        {
            // Act
            ExceptionFactory.ThrowIf(
                false,
                () => new NullReferenceException());

            // Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        public void Character_name_added_to_exception_data()
        {
            // Arrange
            NullReferenceException exception = null;
            int id = 55;

            var componentMock = new Mock<IComponent>();
            componentMock.SetupGet(component => component.Id).Returns(id);

            // Act
            try
            {
                ExceptionFactory.ThrowIf(
                    true,
                    () => new NullReferenceException(),
                    componentMock.Object);
            }
            catch(NullReferenceException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsTrue(exception.Data.Contains("ComponentType"), "Does not have the component Type");
            Assert.IsTrue(exception.Data.Contains("ComponentId"), "Does not have the component id");
            Assert.AreEqual(componentMock.Object.GetType().Name, exception.Data["ComponentType"]);
            Assert.AreEqual(id.ToString(), exception.Data["ComponentId"]);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        public void Custom_data_name_added_to_exception_data()
        {
            // Arrange
            NullReferenceException exception = null;
            var data = new KeyValuePair<string, string>("Key", "Value");

            // Act
            try
            {
                ExceptionFactory.ThrowIf(
                    true,
                    () => new NullReferenceException(),
                    null,
                    data);
            }
            catch (NullReferenceException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsTrue(exception.Data.Contains(data.Key));
            Assert.AreEqual(data.Value, exception.Data[data.Key]);
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        public void Timestamp_added_to_exception_data()
        {
            // Arrange
            NullReferenceException exception = null;

            // Act
            try
            {
                ExceptionFactory.ThrowIf(
                    true,
                    () => new NullReferenceException());
            }
            catch (NullReferenceException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsTrue(exception.Data.Contains("Date"));
            Assert.IsInstanceOfType(Convert.ToDateTime(exception.Data["Date"]), typeof(DateTime));
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        public void Exception_thrown_with_custom_message()
        {
            // Arrange
            NullReferenceException exception = null;

            // Act
            try
            {
                ExceptionFactory.ThrowIf<NullReferenceException>(true, "Custom Message");
            }
            catch (NullReferenceException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsTrue(exception.Message.Contains("Custom Message"));
        }

        [TestMethod]
        [TestCategory("Runtime - ExceptionFactory")]
        public void Data_added_to_existing_exception()
        {
            // Arrange
            NullReferenceException exception = null;
            try
            {
                ExceptionFactory.ThrowIf<NullReferenceException>(true, "Custom Message");
            }
            catch (NullReferenceException ex)
            {
                exception = ex;
            }

            var data = new KeyValuePair<string, string>("Key", "Value");

            // Arrange
            ExceptionFactory.AddExceptionData(exception, data);

            // Assert
            Assert.IsTrue(exception.Data.Contains(data.Key));
            Assert.AreEqual(data.Value, exception.Data[data.Key]);
        }
    }
}
