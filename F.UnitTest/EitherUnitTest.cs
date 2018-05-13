using Ef;
using System;
using Xunit;
using Xunit.Abstractions;

// exposes Option class, BUT not Ef.Option{ None, Some }
using static Ef.F;

namespace F.UnitTest
{
    public class EitherUnitTest
    {
        private ITestOutputHelper _output;

        public EitherUnitTest(ITestOutputHelper o)
        {
            this._output = o;
        }

        [Fact]
        public void Either_create_Left_string_create_Right_int()
        {
            // Arrange

            // calls static generic method Right inside of Ef.F
            var right = Right(12); // calls right<int>, creates a new struct Ef.Either.Right, and puts 12 in it

            var left = Left("oops");

            // Act

            // Assert
            _output.WriteLine($@"");
        }

        [Fact]
        public void Either_match()
        {
            // Arrange

            // calls Left<string>, creates a new struct Ef.Either.Left, and puts "oops" in it
            var left_ = Left("oops");
            Either<string, double> left = Left("oops");

            // calls Right<int>, creates a new struct Ef.Either.Right, and puts 12 in it
            var right_ = Right(12d);
            Either<string, double> right = Right(12d);

            string Render(Either<string, double> val) =>
                val.Match(
                    l => $@"left {l}",
                    r => $@"right {r}"
                );

            var matchResultLeft = Render(left);
            var matchResultRight = Render(right);

            // Act

            // Assert
            _output.WriteLine($@"matchResultLeft : {matchResultLeft  }");
            _output.WriteLine($@"matchResultRight : {matchResultRight }");
        }

        private Either<string, double> Calc(double x, double y)
        {
            if (y == 0)
                return "y cannot be 0 ";

            if (x != 0 && Math.Sign(x) != Math.Sign(y))
                return "z/y cannot be negative ";

            return Math.Sqrt(x / y);
        }

        [Fact]
        public void Either_calc_with_demo_lifting()
        {
            // Arrange

            var ac1 = Calc(3, 0);
            var ac2 = Calc(-3, 3);
            var ac3 = Calc(-3, -3);

            // Act
            var ac1Ret = ac1.Match(l => $"{l}", r => $"{r}");
            var ac2Ret = ac2.Match(l => $"{l}", r => $"{r}");
            var ac3Ret = ac3.Match(l => $"{l}", r => $"{r}");

            // Assert
            _output.WriteLine($@"ac1: {ac1Ret  }");
            _output.WriteLine($@"ac2: {ac2Ret  }");
            _output.WriteLine($@"ac3: {ac3Ret  }");
        }
    }
}