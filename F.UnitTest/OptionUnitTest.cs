using System;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

//using Ef2; // exposes Option class, BUT not Ef2.Option{ None, Some }
//using static Ef2.F;

using Ef; // exposes Option class, BUT not Ef2.Option{ None, Some }
using static Ef.F;


namespace FUnitTest
{

    public class OptionUnitTest
    {
        private readonly ITestOutputHelper output;

        public OptionUnitTest(ITestOutputHelper output_)
        {
            this.output = output_;
        }

        [Fact]
        public void None_is_a_singleton()
        {
            // Arrange
            // get None
            var _ = None; // /*Ef2.F.*/
            var _1 = None;
            var _2 = None;

            // Act

            // Assert
            Assert.Equal(_, _);
            Assert.Equal(_, _1);
            Assert.Equal(_, _2);
        }

        [Fact]
        public void SomeT_creation_you_can_put_non_null_into_the_Some_T()
        {
            // Arrange

            // get None
            var john = Some("John");
            var twoBe = Some(true);
            // Act

            // Assert
            //Assert.NotNull(john);
            //Assert.NotNull(twoBe);
        }

        [Fact]
        public void OptionT_creation_you_cannot_put_a_null_into_the_Some_T()
        {
            // Arrange

            // Act
            Exception ex = Assert.Throws<ArgumentNullException>(() =>Ef.F.Some(null as object));

            // Assert
            //Assert.True(ex.Message.Contains("Cannot wrap"));
        }

        [Fact]
        public void assign_None_to_SomeT_should_carry_a_None_with_no_value()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => Some(null as object));

            // Assert
            //Assert.True(ex.Message.Contains("Cannot wrap"));
        }

        [Fact]
        public void assign_value_to_SomeT_should_carry_none_null_value()
        {
            // Arrange
            var _ = None;

            Option<string> optionString = Some("JOHN");
            Option<bool> optionBool = Some(false);

            // Act

            // Assert
            //var x = optionString.;
        }

        [Fact]
        public void lift_value_to_option(){
            // Arrange

            string null_ = null as string ;
            Option<object> optionNull = null_;

            var o = new object();
            Option<object> optionObject = o;

            Option<string> optionStringNone = None;
            Option<string> optionString = "John";

            // Act
            var actualValue = optionString.Match(() => "NONE", v => v);
            var actualValueNone = optionStringNone.Match(() => "NONE", v => v);

            // Assert
            Assert.False(null_ is string) ;

            Assert.True(actualValue == "John");
            Assert.True(actualValueNone == "NONE");
        }

        [Fact]
        public void get_value_to_SomeT_should_yield_expected()
        {
            // Arrange
            var _ = None;

            Option<string> optionString = Some("JOHN");

            // Act

            //var actualValue =  Match.
            // Assert
            //var x = optionString.;
        }
    }
}