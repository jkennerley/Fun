// exposes Option class, BUT not Ef.Option{ None, Some }

using Fun;
using System;
using Xunit;
using Xunit.Abstractions;

//using static Fun.Create;
using static Fun.Create;

//using Unit = System.ValueTuple;

namespace FunUnitTest
{
    public class EitherUnitTest
    {
        private ITestOutputHelper _output;

        public EitherUnitTest(ITestOutputHelper o)
        {
            this._output = o;
        }

        #region either basics

        ///[Fact]
        public void Either_create_a_Left_string_should_tostring_expected()
        {
            // Arrange

            // calls Fun.Create.Left<string>
            //  -> Either.Left<string>
            var left = Left("oops");

            // Act

            var actual = left.ToString();

            //_output.WriteLine($@" left {actual}");

            // Assert
            Assert.Equal("Left(oops)", actual);
        }

        ///[Fact]
        public void Either_create_a_Right_of_int__should_tostring_expected()
        {
            // Arrange

            // calls Fun.Create.Right<string>
            //  -> Either.Right<int>
            var right = Right(12);

            // Act

            var actual = right.ToString();

            //_output.WriteLine($@" right {actual}");

            // Assert
            Assert.Equal("Right(12)", actual);
        }

        //[Fact]
        public void assign_left_value_to_eitherLR_should_cause_either_to_left_state()
        {
            // Arrange

            // Act

            // assign a Left value to a Either value,
            Either<string, double> eitherLifted = "oops";
            Either<string, double> either = Left("oops");

            // Assert
            Assert.True(eitherLifted.IsLeft);
            Assert.False(eitherLifted.IsRight);

            Assert.True(either.IsLeft);
            Assert.False(either.IsRight);
        }

        //[Fact]
        public void assign_right_value_to_eitherLR_should_cause_either_to_in_right_state()
        {
            // Arrange

            // Act
            Either<string, double> eitherLifted = 12.0;
            Either<string, double> either = Right(12.0);

            // Assert
            Assert.False(eitherLifted.IsLeft);
            Assert.True(eitherLifted.IsRight);

            Assert.False(either.IsLeft);
            Assert.True(either.IsRight);
        }

        public Either<string, double> CreateEitherOfStringDouble(double right)
        {
            Either<string, double> either = right;
            return either;
        }

        public Either<string, double> CreateEitherOfStringDouble(string left)
        {
            Either<string, double> either = left;
            return either;
        }

        //[Fact]
        public void
            with_biz_use_case_crreate_function_assignment_should_cause_either_to_in_expected_left_or_right_state()
        {
            // Arrange

            // Act
            var leftEither = CreateEitherOfStringDouble("oops");
            var rightEither = CreateEitherOfStringDouble(12.0);

            // Assert
            Assert.True(leftEither.IsLeft);
            Assert.False(leftEither.IsRight);

            Assert.False(rightEither.IsLeft);
            Assert.True(rightEither.IsRight);
        }

        // renders an either, via the  Match function
        private string RenderToString(Either<string, double> _either) =>
            _either.Match(
                l => $@"left {l}",
                r => $@"right {r}"
            );

        // renders an either, via the  Match function
        private static string RenderToString(Either<string, int> e) =>
            e.Match(
                l => $@"left {l}",
                r => $@"right {r}"
            );

        //[Fact]
        public void a_left_either_when_matched_should_call_the_left_lambda()
        {
            // Arrange

            // an either in the left state
            Either<string, double> either = "oops";

            // Act

            // sut call : the match function is via the Render function
            var renderingOfTheEither = RenderToString(either);

            // Assert

            // should have rendered oops here, because the either was in the left state
            Assert.Equal("left oops", renderingOfTheEither);
        }

        //[Fact]
        public void a_right_either_when_matched_should_call_the_right_lambda()
        {
            // Arrange

            // an either in the left state
            Either<string, double> either = 12.0;

            // Act

            // sut call : the match function is via the Render function
            var renderingOfTheEither = RenderToString(either);

            // Assert

            // should have rendered oops here, because the either was in the left state
            Assert.Equal("right 12", renderingOfTheEither);
        }

        /// <summary>
        //// takes a double -> double
        ////  returns Either<string,double>
        //// if  y is zero              => then will be in left-fail state
        //// if  x is zero & x,y have opposite signs => then will be in left-fail state
        //// else happy path ... with returning Either in success state
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Either<string, double> Calc(double x, double y)
        {
            if (y == 0.0)
            {
                // return left-fail
                return "y cannot be 0";
            }

            // if x not ZERO
            // then  x and y should have the same sign
            if (
                x != 0.0
                && Math.Sign(x) != Math.Sign(y)
            )
            {
                // return fail-left
                return "x/y cannot be negative";
            }

            // return Success
            return Math.Sqrt(x / y);
        }

        //[Fact]
        public void Either_calc_with_demo_lifting()
        {
            // Arrange

            // y is zero so expect left-fail
            var either1 = Calc(3, 0);

            // x = y have different sign
            var either2 = Calc(-3, 3);

            // x = y have different sign
            var either3 = Calc(0, 3);

            // x = y have same sign, so => right-success
            var either4 = Calc(-3, -3);
            var either5 = Calc(3, 3);

            // Act

            // Assert
            Assert.True(either1.IsLeft);
            // ... the left  is carrying info about the fail
            var render1 = either1.Match(l => $"{l}", r => $"{r}");
            Assert.Equal($@"y cannot be 0", render1);

            //
            Assert.True(either2.IsLeft);
            // ... the left  is carrying info about the fail
            var render2 = either2.Match(l => $"{l}", r => $"{r}");
            Assert.Equal($@"x/y cannot be negative", render2);
            // ... the left  is carrying info about the fail

            Assert.True(either3.IsRight);
            Assert.True(either4.IsRight);
            Assert.True(either5.IsRight);
        }

        #endregion either basics

        #region exp 2

        //[Fact]
        public void Either_comparing_with_render_should_expected()
        {
            // Arrange

            // an either in the left state
            Either<string, int> either = 1;

            // Act

            // sut call : the match function is via the Render function
            var renderingOfTheEither = RenderToString(either);

            // Assert

            // should have rendered oops here, because the either was in the left state
            Assert.Equal("right 1", renderingOfTheEither);
        }

        [Fact]
        public void Either_comparing_with_default_coamparer_should_expected()
        {
            // Arrange

            // Act
            Either<string, int> either = 1;

            // Assert
            Either<string, int> swappedToEither = 1;
            Assert.Equal(swappedToEither, either);
            //var isSame = ( swappedToEither == either);
        }

        public class RenderEitherWithMatch
        {
            private ITestOutputHelper _output;

            public RenderEitherWithMatch(ITestOutputHelper o) => this._output = o;

            //public void RenderWithSide(Either<string, int> either)
            //{
            //    either.Match(
            //        l => this._output.WriteLine($" fail {l} "),
            //        r => this._output.WriteLine($" success {r} ")
            //    );
            //}

            public static string Render(Either<string, int> either)
            {
                return either.Match(
                    l => ($" fail {l} "),
                    r => ($" success {r} ")
                );
            }

            [Fact]
            public void render_left_either_should_yield_expected_lefty_value()
            {
                // Arrange
                Either<string, int> either = ":-(";

                // Act

                var rendered = Render(either);
                // Assert
                Assert.Contains(":-(", rendered);
            }

            [Fact]
            public void render_right_either_should_yield_expected_righty_value()
            {
                // Arrange

                Either<string, int> either = 1;

                // Act
                var rendered = Render(either);

                // Assert
                Assert.Contains("1", rendered);
            }

            [Fact]
            public void Either_Map_of_right_should_yield_expected_right_value()
            {
                // Arrange
                Either<string, int> either = 2;

                // Act

                // this will convert an
                //     Either<sting,int>
                //  to
                //    convert an Either<sting,double>
                var actual =
                    either
                    .Map(Convert.ToDouble);

                // Assert
                Assert.Equal(2.0, actual);
            }

            [Fact]
            public void EitherMap_of_left_should_print_left_value()
            {
                // Arrange
                Either<string, int> either = "sad-value :-(";

                // Act

                var actual = either
                    .Map(Convert.ToDouble);

                // Assert
                Assert.Equal("sad-value :-(", actual);
            }

            public class MakeToffeeApple
            {
                private ITestOutputHelper _output;

                public MakeToffeeApple(ITestOutputHelper o) =>
                    this._output = o;

                public class Ingredients
                {
                    public string Apple;
                }

                public static bool validateAppleReq => true;

                public static Either<string, Ingredients> validateBic(Ingredients ingredients)
                {
                    if (!ingredients.Apple.Contains("red-apple"))
                    {
                        return "not a red apple!";
                    }

                    return ingredients;
                }
                public static Either<string, Ingredients> normaliseBic(Ingredients ingredients)
                {
                    return  new Ingredients
                    {
                        Apple = ingredients.Apple.Trim()
                    };
                }

                [Fact]
                public void EitherMap_of_right_should_proint_right_value()
                {
                    // Arrange
                    Either<string, Ingredients> either =
                        new Ingredients
                        {
                            Apple = "  red-apple  "
                        };

                    // Act

                    var productBag = either.
                        Bind(validateBic).
                        Bind(normaliseBic);

                    // Assert
                    var actualProductContent = productBag.Match(l => l, r => r.Apple);
                    Assert.Equal("red-apple", actualProductContent);
                }


                [Fact]
                public void EitherMap_of_left_should_proint_right_value()
                {
                    // Arrange
                    Either<string, Ingredients> either =
                        new Ingredients
                        {
                            Apple = "  green-apple  "
                        };

                    // Act

                    var productBag = either.
                        Bind(validateBic);

                    // Assert
                    var actualProductContent = productBag.Match(l => l, r => r.Apple);
                    Assert.Equal("not a red apple!", actualProductContent);
                }

            }

            #endregion exp 2
        }
    }
}