using Ef;
using System;
using Xunit;
using Xunit.Abstractions;

// exposes Option class, BUT not Ef.Option{ None, Some }
using static Fun.F;
//using Unit = System.ValueTuple;

namespace F.UnitTest
{
    public class EitherUnitTest
    {
        private ITestOutputHelper _output;

        public EitherUnitTest(ITestOutputHelper o)
        {
            this._output = o;
        }

        #region exp 2

        //public class EitherCreateUnitTest
        //{
        //    private ITestOutputHelper _output;
        //
        //    public EitherCreateUnitTest(ITestOutputHelper o) => this._output = o;
        //
        //    [Fact]
        //    public Unit Either_create_Right_int()
        //    {
        //        // Arrange
        //        // Act
        //        var right = Right(1); // Ef.Either.Right()
        //        // Assert
        //        Assert.Equal(Right(1), right);
        //        return Unit();
        //    }
        //
        //    [Fact]
        //    public void Either_create_Either_string_int()
        //    {
        //        // Arrange
        //
        //        // Act
        //        Either<string, int> right_ = Right(1);
        //
        //        // Assert
        //        Assert.Equal(Right(1), right_);
        //    }
        //}

        //public class RenderEitherWithMatch
        //{
        //    private ITestOutputHelper _output;
        //
        //    public RenderEitherWithMatch(ITestOutputHelper o) => this._output = o;
        //
        //    public void RenderWithSide(Either<string, int> either)
        //    {
        //        either.Match(
        //            l => this._output.WriteLine($" fail {l} "),
        //            r => this._output.WriteLine($" success {r} ")
        //        );
        //    }
        //
        //    public static string Render(Either<string, int> either)
        //    {
        //        var ret = either.Match(
        //            l => ($" fail {l} "),
        //            r => ($" success {r} ")
        //        );
        //        return ret;
        //    }
        //
        //    [Fact]
        //    public void render_left_either_should_yield_expected_lefty_value()
        //    {
        //        // Arrange
        //        Either<string, int> either = Left("oops");
        //
        //        // Act
        //        var rendered = Render(either);
        //
        //        // Assert
        //        Assert.Contains("oops", rendered);
        //    }
        //
        //    [Fact]
        //    public void render_right_either_should_yield_expected_righty_value()
        //    {
        //        // Arrange
        //        Either<string, int> either = Right(1);
        //        // Act
        //        var rendered = Render(either);
        //
        //        // Assert
        //        Assert.Contains("1", rendered);
        //    }
        //}

        //public class EitherMap
        //{
        //    private ITestOutputHelper _output;
        //
        //    public EitherMap(ITestOutputHelper o) => this._output = o;
        //
        //    [Fact]
        //    public void EitherMap_of_right_should_proint_right_value()
        //    {
        //        // Arrange
        //        Either<string, int> req = Right(1);
        //
        //        // Act
        //        var ac =
        //            req
        //                .Map(x => Convert.ToDouble(x));
        //
        //        // Assert
        //        Assert.Equal(Right(1.0), ac);
        //    }
        //
        //    [Fact]
        //    public void EitherMap_of_left_should_print_left_value()
        //    {
        //        // Arrange
        //        Either<string, int> req = Left("oops");
        //
        //        // Act
        //        var ac =
        //            req
        //                .Map(Convert.ToDouble);
        //
        //        // Assert
        //        Assert.Equal(Left("oops"), ac);
        //    }
        //
        //}

        //public class Either_Map_Apple_Validate_Normalise_Bake_Pack
        //{
            //private ITestOutputHelper _output;
            //
            //public Either_Map_Apple_Validate_Normalise_Bake_Pack(ITestOutputHelper o) => this._output = o;
            //
            //public class AppleReq
            //{
            //    public string Apple;
            //}
            //
            //public static bool validateAppleReq => true;
            ////public static Either<string,AppleReq>  validateAppleReqWithLift( Either<string, AppleReq> req ) => req.Match( l=> Left(l) , r =>    )                ;
            //
            //
            //public static AppleReq normaliseAppleReq(AppleReq req)
            //{
            //    return new AppleReq {Apple = req.Apple.Trim()};
            //}



            //[Fact]
            //public void EitherMap_of_right_should_proint_right_value()
            //{
            //    // Arrange
            //    var appleReq = new AppleReq {Apple = "  green-apple  "};
            //    var right = Right(appleReq);
            //    Either<string, AppleReq> req = right;
            //
            //    // Act
            //
            //    //var ac =
            //    //    req
            //    //    .Where(x => validateAppleReq)
            //    //    ;
            //
            //    // Assert
            //    //Assert.Equal(Right(1.0), ac);
            //}

            //[Fact]
            //public void EitherMap_of_left_should_print_left_value()
            //{
            //    // Arrange
            //    Either<string, int> req = Left("oops");
            //
            //    // Act
            //    var ac =
            //        req
            //            .Map(Convert.ToDouble);
            //
            //    // Assert
            //    Assert.Equal(Left("oops"), ac);
            //}

        //}


        #endregion exp 2


        #region exp 1

        [Fact]
        public void Either_create_Left_string_create_Right_int()
        {
            // Arrange
        
            // calls static generic method Right inside of Ef.F
            //var right = Right(12); // calls right<int>, creates a new struct Ef.Either.Right, and puts 12 in it
        
            //var left = Left("oops");
        
            // Act
        
            // Assert
            _output.WriteLine($@"");
        }
        
        //[Fact]
        //public void Either_match()
        //{
        //    // Arrange
        //
        //    // calls Left<string>, creates a new struct Ef.Either.Left, and puts "oops" in it
        //    var left_ = Left("oops");
        //    Either<string, double> left = Left("oops");
        //
        //    // calls Right<int>, creates a new struct Ef.Either.Right, and puts 12 in it
        //    var right_ = Right(12d);
        //    Either<string, double> right = Right(12d);
        //
        //    string Render(Either<string, double> val) =>
        //        val.Match(
        //            l => $@"left {l}",
        //            r => $@"right {r}"
        //        );
        //
        //    var matchResultLeft = Render(left);
        //    var matchResultRight = Render(right);
        //
        //    // Act
        //
        //    // Assert
        //    _output.WriteLine($@"matchResultLeft : {matchResultLeft  }");
        //    _output.WriteLine($@"matchResultRight : {matchResultRight }");
        //}
        
        //private Either<string, double> Calc(double x, double y)
        //{
        //    if (y == 0)
        //        return "y cannot be 0 ";
        //
        //    if (x != 0 && Math.Sign(x) != Math.Sign(y))
        //        return "z/y cannot be negative ";
        //
        //    return Math.Sqrt(x / y);
        //}
        
        //[Fact]
        //public void Either_calc_with_demo_lifting()
        //{
        //    // Arrange
        //
        //    var ac1 = Calc(3, 0);
        //    var ac2 = Calc(-3, 3);
        //    var ac3 = Calc(-3, -3);
        //
        //    // Act
        //    var ac1Ret = ac1.Match(l => $"{l}", r => $"{r}");
        //    var ac2Ret = ac2.Match(l => $"{l}", r => $"{r}");
        //    var ac3Ret = ac3.Match(l => $"{l}", r => $"{r}");
        //
        //    // Assert
        //    _output.WriteLine($@"ac1: {ac1Ret  }");
        //    _output.WriteLine($@"ac2: {ac2Ret  }");
        //    _output.WriteLine($@"ac3: {ac3Ret  }");
        //}

        #endregion exp 1
    }
}