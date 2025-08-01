namespace RA.Utilities.Test.ErrorHandling
{
    public class CustomErrorHandlerBaseTest
    {
        [Fact]
        public void IsIgnored_WhenExceptionTypeIsIgnored_ShouldReturnTrue()
        {
            // Arrange
            var customErrorHandler = new FakeCustomErrorHandlerBase(typeof(ArgumentException));
            // Act
            var result = customErrorHandler.IsIgnored(typeof(ArgumentException));
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsIgnored_WhenExceptionTypeIsNotIgnored_ShouldReturnFalse()
        {
            // Arrange
            var customErrorHandler = new FakeCustomErrorHandlerBase(typeof(ArgumentException));
            // Act
            var result = customErrorHandler.IsIgnored(typeof(InvalidOperationException));
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Ignore_WhenAddingExceptionType_ShouldAddToIgnoredList()
        {
            // Arrange
            var customErrorHandler = new FakeCustomErrorHandlerBase();
            // Act
            customErrorHandler.Ignore(typeof(ArgumentException));
            // Assert
            Assert.True(customErrorHandler.IsIgnored(typeof(ArgumentException)));
        }

        [Fact]
        public void NotIgnore_WhenRemovingExceptionType_ShouldRemoveFromIgnoredList()
        {
            // Arrange
            var customErrorHandler = new FakeCustomErrorHandlerBase(typeof(ArgumentException));
            // Act
            customErrorHandler.NotIgnore(typeof(ArgumentException));
            // Assert
            Assert.False(customErrorHandler.IsIgnored(typeof(ArgumentException)));
        }

        [Fact]
        public void ClearIgnored_WhenCalled_ShouldClearAllIgnoredExceptions()
        {
            // Arrange
            var customErrorHandler = new FakeCustomErrorHandlerBase(typeof(ArgumentException), typeof(InvalidOperationException));
            // Act
            customErrorHandler.ClearIgnored();
            // Assert
            Assert.Empty(customErrorHandler.GetIgnoredExceptions());
        }

        [Fact]
        public void GetIgnoredExceptions_WhenCalled_ShouldReturnListOfIgnoredExceptions()
        {
            // Arrange
            var customErrorHandler = new FakeCustomErrorHandlerBase(typeof(ArgumentException), typeof(InvalidOperationException));
            // Act
            var ignoredExceptions = customErrorHandler.GetIgnoredExceptions();
            // Assert
            Assert.Contains(typeof(ArgumentException), ignoredExceptions);
            Assert.Contains(typeof(InvalidOperationException), ignoredExceptions);
        }

        [Fact]
        public void GetIgnoredExceptions_WhenNoExceptionsIgnored_ShouldReturnEmptyList()
        {
            // Arrange
            var customErrorHandler = new FakeCustomErrorHandlerBase();
            // Act
            var ignoredExceptions = customErrorHandler.GetIgnoredExceptions();
            // Assert
            Assert.Empty(ignoredExceptions);
        }

        [Fact]
        public void IsIgnored_WhenExceptionTypeInheritsFromIgnoredType_ShouldReturnTrue()
        {
            // Arrange
            var customErrorHandler = new FakeCustomErrorHandlerBase(typeof(ArgumentException));
            // Act
            var result = customErrorHandler.IsIgnored(typeof(ArgumentNullException));
            // Assert
            Assert.True(result);
        }
    }
}
