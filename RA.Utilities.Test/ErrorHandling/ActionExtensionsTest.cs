using RA.Utilities.ErrorHandling;

namespace RA.Utilities.Test.ErrorHandling
{
    public class ActionExtensionsTest
    {
        [Fact]
        public void TryExecute_WhenActionSucceeds_ShouldReturnTrue()
        {
            // Arrange
            Action action = () => { /* Do nothing */ };
            // Act
            var result = action.TryExecute();
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TryExecute_WhenActionSucceedsAndHasOnError_ShouldReturnTrueAndNoExecuteOnError()
        {
            // Arrange
            Action action = () => { /* Do nothing */ };
            var onErrorExecuted = false;
            // Act
            var result = action.TryExecute((ex) => onErrorExecuted = true);
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionSucceedsAndHasOnErrorAndIgnoredExceptions_ShouldReturnTrueAndNoExecuteOnError()
        {
            // Arrange
            Action action = () => { /* Do nothing */ };
            var onErrorExecuted = false;
            // Act
            var result = action.TryExecute((ex) => onErrorExecuted = true,
                typeof(ArgumentException));
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionSucceedsAndHasParameterAndHasOnError_ShouldReturnTrueAndNoExecuteOnError()
        {
            // Arrange
            Action action = () => { /* Do nothing */ };
            var onErrorExecuted = false;
            var parameter = "test";
            // Act
            var result = action.TryExecute(parameter, (p, ex) => onErrorExecuted = true);
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionSucceedsAndHasParameterAndHasOnErrorAndIgnoredExceptions_ShouldReturnTrueAndNoExecuteOnError()
        {
            // Arrange
            Action action = () => { /* Do nothing */ };
            var onErrorExecuted = false;
            var parameter = "test";
            // Act
            var result = action.TryExecute(parameter, (p, ex) => onErrorExecuted = true,
                typeof(ArgumentException));
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionThrows_ShouldReturnFalse()
        {
            // Arrange
            Action action = () => throw new Exception();
            // Act
            var result = action.TryExecute();
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasOnError_ShouldReturnFalseAndExecuteOnError()
        {
            // Arrange
            Action action = () => throw new Exception();
            var onErrorExecuted = false;
            // Act
            var result = action.TryExecute((ex) => onErrorExecuted = true);
            // Assert
            Assert.False(result);
            Assert.True(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasOnErrorAndDoNotMatchIgnoredExceptions_ShouldReturnFalseAndExecuteOnError()
        {
            // Arrange
            Action action = () => throw new InvalidOperationException();
            var onErrorExecuted = false;
            // Act
            var result = action.TryExecute((ex) => onErrorExecuted = true,
                typeof(ArgumentException));
            // Assert
            Assert.False(result);
            Assert.True(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasOnErrorAndMatchIgnoredExceptions_ShouldReturnTrueAndNotExecuteOnError()
        {
            // Arrange
            Action action = () => throw new ArgumentException();
            var onErrorExecuted = false;
            // Act
            var result = action.TryExecute((ex) => onErrorExecuted = true,
                typeof(ArgumentException));
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasOnErrorAndInheritIgnoredExceptions_ShouldReturnTrueAndNotExecuteOnError()
        {
            // Arrange
            Action action = () => throw new ArgumentNullException();
            var onErrorExecuted = false;
            // Act
            var result = action.TryExecute((ex) => onErrorExecuted = true,
                typeof(ArgumentException));
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasParameterAndHasOnError_ShouldReturnFalseAndExecuteOnErrorWithTheCorrectParameter()
        {
            // Arrange
            Action action = () => throw new Exception();
            var onErrorExecuted = false;
            var parameter = "test";
            var receivedParameter = string.Empty;
            // Act
            var result = action.TryExecute(parameter, (p, ex) => 
            { 
                onErrorExecuted = true;
                receivedParameter = p;
            });
            // Assert
            Assert.False(result);
            Assert.True(onErrorExecuted);
            Assert.Equal(parameter, receivedParameter);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasParameterAndHasOnErrorAndDoNotMatchIgnoredExceptions_ShouldReturnFalseAndExecuteOnErrorWithTheCorrectParameter()
        {
            // Arrange
            Action action = () => throw new InvalidOperationException();
            var onErrorExecuted = false;
            var parameter = "test";
            var receivedParameter = string.Empty;
            // Act
            var result = action.TryExecute(parameter, (p, ex) =>
            {
                onErrorExecuted = true;
                receivedParameter = p;
            }, typeof(ArgumentException));
            // Assert
            Assert.False(result);
            Assert.True(onErrorExecuted);
            Assert.Equal(parameter, receivedParameter);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasParameterAndHasOnErrorAndMatchIgnoredExceptions_ShouldReturnTrueAndNoExecuteOnError()
        {
            // Arrange
            Action action = () => throw new ArgumentException();
            var onErrorExecuted = false;
            var parameter = "test";
            var receivedParameter = string.Empty;
            // Act
            var result = action.TryExecute(parameter, (p, ex) =>
            {
                onErrorExecuted = true;
                receivedParameter = p;
            }, typeof(ArgumentException));
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
            Assert.Equal(string.Empty, receivedParameter);
        }

        [Fact]
        public void TryExecute_WhenActionThrowsAndHasParameterAndHasOnErrorAndInheritIgnoredExceptions_ShouldReturnTrueAndNoExecuteOnError()
        {
            // Arrange
            Action action = () => throw new ArgumentNullException();
            var onErrorExecuted = false;
            var parameter = "test";
            var receivedParameter = string.Empty;
            // Act
            var result = action.TryExecute(parameter, (p, ex) =>
            {
                onErrorExecuted = true;
                receivedParameter = p;
            }, typeof(ArgumentException));
            // Assert
            Assert.True(result);
            Assert.False(onErrorExecuted);
            Assert.Equal(string.Empty, receivedParameter);
        }
    }
}
