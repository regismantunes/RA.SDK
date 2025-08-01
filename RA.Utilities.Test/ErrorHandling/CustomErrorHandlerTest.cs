using RA.Utilities.ErrorHandling;

namespace RA.Utilities.Test.ErrorHandling
{
    public class CustomErrorHandlerTest
    {
        [Fact]
        public void TryExecute_WhenActionSucceeds_ShouldReturnTrue()
        {
            // Arrange
            var customErrorHandler = new CustomErrorHandler();
            // Act
            var result = customErrorHandler.TryExecute(() => { /* Do nothing */ });
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TryExecute_WhenActionSucceedsAndHasOnError_ShouldReturnTrueAndNoExecuteOnError()
        {
            // Arrange
            var onErrorExecuted = false;
            var customErrorHandler = new CustomErrorHandler((ex) => onErrorExecuted = true);
            // Act
            var result = customErrorHandler.TryExecute(() => { /* Do nothing */ });
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionSucceedsAndHasOnErrorAndIgnoredExceptions_ShouldReturnTrueAndNoExecuteOnError()
        {
            // Arrange
            var onErrorExecuted = false;
            var customErrorHandler = new CustomErrorHandler((ex) => onErrorExecuted = true,
                typeof(ArgumentException));
            // Act
            var result = customErrorHandler.TryExecute(() => { /* Do nothing */ });
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionSucceedsAndHasParameterAndHasOnError_ShouldReturnTrueAndNoExecuteOnError()
        {
            // Arrange
            var onErrorExecuted = false;
            var customErrorHandler = new CustomErrorHandler<string>((p, ex) => onErrorExecuted = true);
            var parameter = "test";
            // Act
            var result = customErrorHandler.TryExecute(() => { /* Do nothing */ }, parameter);
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionSucceedsAndHasParameterAndHasOnErrorAndIgnoredExceptions_ShouldReturnTrueAndNoExecuteOnError()
        {
            // Arrange
            var onErrorExecuted = false;
            var customErrorHandler = new CustomErrorHandler<string>((p, ex) => onErrorExecuted = true,
                typeof(ArgumentException));
            var parameter = "test";
            // Act
            var result = customErrorHandler.TryExecute(() => { /* Do nothing */ }, parameter);
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionThrows_ShouldReturnFalse()
        {
            // Arrange
            var customErrorHandler = new CustomErrorHandler();
            // Act
            var result = customErrorHandler.TryExecute(() => throw new Exception());
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasOnError_ShouldReturnFalseAndExecuteOnError()
        {
            // Arrange
            var onErrorExecuted = false;
            var customErrorHandler = new CustomErrorHandler((ex) => onErrorExecuted = true);
            // Act
            var result = customErrorHandler.TryExecute(() => throw new Exception());
            // Assert
            Assert.False(result);
            Assert.True(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasOnErrorAndDoNotMatchIgnoredExceptions_ShouldReturnFalseAndExecuteOnError()
        {
            // Arrange
            var onErrorExecuted = false;
            var customErrorHandler = new CustomErrorHandler((ex) => onErrorExecuted = true,
                typeof(ArgumentException));
            // Act
            var result = customErrorHandler.TryExecute(() => throw new InvalidOperationException());
            // Assert
            Assert.False(result);
            Assert.True(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasOnErrorAndMatchIgnoredExceptions_ShouldReturnTrueAndNotExecuteOnError()
        {
            // Arrange
            var onErrorExecuted = false;
            var customErrorHandler = new CustomErrorHandler((ex) => onErrorExecuted = true,
                typeof(ArgumentException));
            // Act
            var result = customErrorHandler.TryExecute(() => throw new ArgumentException());
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasOnErrorAndInheritIgnoredExceptions_ShouldReturnTrueAndNotExecuteOnError()
        {
            // Arrange
            var onErrorExecuted = false;
            var customErrorHandler = new CustomErrorHandler((ex) => onErrorExecuted = true,
                typeof(ArgumentException));
            // Act
            var result = customErrorHandler.TryExecute(() => throw new ArgumentNullException());
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasParameterAndHasOnError_ShouldReturnFalseAndExecuteOnErrorWithTheCorrectParameter()
        {
            // Arrange
            var onErrorExecuted = false;
            var parameter = "test";
            var receivedParameter = string.Empty;
            var customErrorHandler = new CustomErrorHandler<string>((p, ex) =>
            {
                onErrorExecuted = true;
                receivedParameter = p;
            }, typeof(ArgumentException));
            // Act
            var result = customErrorHandler.TryExecute(() => throw new Exception(), parameter);
            // Assert
            Assert.False(result);
            Assert.True(onErrorExecuted);
            Assert.Equal(parameter, receivedParameter);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasParameterAndHasOnErrorAndDoNotMatchIgnoredExceptions_ShouldReturnFalseAndExecuteOnErrorWithTheCorrectParameter()
        {
            // Arrange
            var onErrorExecuted = false;
            var parameter = "test";
            var receivedParameter = string.Empty;
            var customErrorHandler = new CustomErrorHandler<string>((p, ex) =>
            {
                onErrorExecuted = true;
                receivedParameter = p;
            }, typeof(ArgumentException));
            // Act
            var result = customErrorHandler.TryExecute(() => throw new InvalidOperationException(), parameter);
            // Assert
            Assert.False(result);
            Assert.True(onErrorExecuted);
            Assert.Equal(parameter, receivedParameter);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasParameterAndHasOnErrorAndMatchIgnoredExceptions_ShouldReturnTrueAndNoExecuteOnError()
        {
            // Arrange
            var onErrorExecuted = false;
            var parameter = "test";
            var receivedParameter = string.Empty;
            var customErrorHandler = new CustomErrorHandler<string>((p, ex) =>
            {
                onErrorExecuted = true;
                receivedParameter = p;
            }, typeof(ArgumentException));
            // Act
            var result = customErrorHandler.TryExecute(() => throw new ArgumentException(), parameter);
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
            Assert.Equal(string.Empty, receivedParameter);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasParameterAndHasOnErrorAndInheritIgnoredExceptions_ShouldReturnTrueAndNoExecuteOnError()
        {
            // Arrange
            var onErrorExecuted = false;
            var parameter = "test";
            var receivedParameter = string.Empty;
            var customErrorHandler = new CustomErrorHandler<string>((p, ex) =>
            {
                onErrorExecuted = true;
                receivedParameter = p;
            }, typeof(ArgumentException));
            // Act
            var result = customErrorHandler.TryExecute(() => throw new ArgumentNullException(), parameter);
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
            Assert.Equal(string.Empty, receivedParameter);
        }
    }
}
