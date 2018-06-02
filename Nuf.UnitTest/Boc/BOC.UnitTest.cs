using Nuf.Boc;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Nuf.UnitTest
{
    public class BOCUnitTest
    {
        private readonly ITestOutputHelper _output;

        public BOCUnitTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        private static DateTime presentDate = new DateTime(2016, 12, 12);

        [Fact]
        public void valid_when_date_is_in_the_past_should_be_false()
        {
            // Arrange
            //var fake = new Mock<IDateTimeService>();
            //fake.SetupGet(x => x.UtcNow).Returns(presentDate);

            //var sut = new DateNotPastValidator(fake.Object);
            var sut = new DateNotPastValidator(presentDate);

            var cmd = new MakeTransfer { Date = presentDate.AddDays(-1) };

            // Act

            var actual = sut.IsValid(cmd);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData("A", true)]
        [InlineData("B", false)]
        public void bic_exists_valid_should_be_expected(string bic, bool expected)
        {
            // Arrange
            string[] validCodes = { "A" };

            var cmd = new MakeTransfer { Bic = bic };

            var sut = new BicExistsValidator(() => validCodes);

            // Act
            var actual = sut.IsValid(cmd);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}