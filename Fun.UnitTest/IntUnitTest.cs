using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FunUnitTest
{
    public class IntUnitTest
    {
        private ITestOutputHelper output;

        public IntUnitTest( ITestOutputHelper o_)
        {
            this.output = o_;
        }

        [Fact]
        void asb()
        {
            // Arrange
            
            // Act

            // Assert
            Assert.True(true);
        }

    }
}
