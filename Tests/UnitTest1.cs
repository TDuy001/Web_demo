

using Xunit;

namespace Demo3.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            int expected = 10;

            // Act
            int actual = 5 + 5;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
