using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static System.Console;

using static System.Linq.Enumerable ; 

namespace Nuf.UnitTest.Funcy
{
    public class TenetsUnitTest
    {
        private ITestOutputHelper output;

        public TenetsUnitTest(ITestOutputHelper output)
        {
            this.output = output;
            //this.output.WriteLine($@"XXXXXXXXXXX ");
        }

        [Fact]
        public void demo_how_inplace_sort_call_from_2nd_thread_causes_1st_thread_to_get_wrone_result()
        {
            // Arrange

            var start = -10000;
            var N = ((-1*start) * 2 + 1 ) ;

            var nums =
                Range(start , N)
                .Reverse()
                .ToList();

            Action task1 = () =>
            {
                // goes wrong because another code line is doing an in place sort while this is summing 
                WriteLine( $"sum-1: {nums.Sum()} " );
            };

            Action task2 = () =>
            {
                nums.Sort();// thus jests up the other sum
                WriteLine($"sum-2 : {nums.Sum()}");
            };

            Action task3 = () =>
            {
                WriteLine($"sum-3 : {nums.OrderBy(x=>x).Sum()}");
            };

            // Act

            // this means the task1 sum result will most likely be wrong. depending on the size of the array
            //Parallel.Invoke(task1, task2, task3);

            // this means the task1 sum result will always be correct
            Parallel.Invoke(task1, task3);

            // Assert
            //Assert.True(1==1);
        }
    }
}
