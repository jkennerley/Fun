using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ef;
using Xunit;
using Xunit.Abstractions;

namespace F.UnitTest
{
    public class EnumerableExtUnitTest
    {
        private ITestOutputHelper output;

        public EnumerableExtUnitTest(ITestOutputHelper o_)
        {
            this.output = o_;
        }

        [Fact]
        public void Map_can_be_synonym_for_Select()
        {
            // Arrange
            var xs  = Enumerable.Range(1, 3); 

            // Act

            var ys =  
                xs
                .Map(x => x * 3)
                .ToList();

            // Assert
            Assert.True(ys.Count() == 3);
            Assert.True(ys[0] == 3);
            Assert.True(ys[1] == 2*3);
            Assert.True(ys[2] == 3*3);

        }
    }
}
