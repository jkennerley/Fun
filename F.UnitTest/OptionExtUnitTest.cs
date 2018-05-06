using System;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

//using Ef2; // exposes Option class, BUT not Ef2.Option{ None, Some }
//using static Ef2.F;

using Ef; // exposes Option class, BUT not Ef2.Option{ None, Some }
using static Ef.F;
using static Ef.OptionExt;


namespace FUnitTest
{

    public class OptionExtUnitTest
    {
        private readonly ITestOutputHelper output;

        public OptionExtUnitTest(ITestOutputHelper output_)
        {
            this.output = output_;
        }

        [Fact]
        public void map_fun_returning_string_to_return_ofoption_string()
        {
            // Arrange

            // greeter : string -> string
            Func<string, string> greeter = (s) => $@"hello world {s}";

            Option<string> _ = None;
            Option<string> jk = Some("jk");

            // Act

            // Option<string> -> Map of greeter's return wrapped in Option
            var ac1 = _.Map(greeter);
            // Option<string> -> Map of greeter's return wrapped in Option
            var ac2 = jk.Map(greeter);

            // Assert
            Assert.Equal(None, ac1);
            Assert.Equal(Some("hello world jk"), ac2);

        }


    }
}