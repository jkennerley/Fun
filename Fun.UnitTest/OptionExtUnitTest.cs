using System;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

//using Ef2; // exposes Option class, BUT not Ef2.Option{ None, Some }
//using static Ef2.F;

using Fun; // exposes Option class, BUT not Ef2.Option{ None, Some }
using static Fun.F;
using static Fun.OptionExt;


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


        public class Apple{}
        public class Pie{}
        Func<Apple, Pie> makePie  = (apple) => new Pie();

        [Fact]
        public void if_non_empty_bag_of_apples_then_makepies_should_create_bag_of_apple_pies_else_no_pies()
        {
            // Arrange
            var apple = new Apple();

            var appleBag1 = Some(apple);

            Option<Apple> appleBag2 = None ;

            // Act
            var pieBag1 = appleBag1.Map(makePie);
            var pieBag2 = appleBag2.Map(makePie);

            var couldBePie1 = pieBag1.Match(() => null, (pie) => pie);
            var couldBePie2 = pieBag2.Match(() => null, (pie) => pie);

            // Assert
            Assert.NotNull(couldBePie1);
            Assert.Null(couldBePie2);
        }
    }


}