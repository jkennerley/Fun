using Xunit;
using Xunit.Abstractions;

using static System.Math;

namespace Nuf.UnitTest.Chapter01
{
    public class Circle
    {
        // read-only property
        public double Radius { get; }

        public Circle(double radius)
        {
            this.Radius = radius;
        }

        // expression bodied property, calculated on each call
        public double Circumference => PI * 2 * Radius;

        public double Area
        {
            get
            {
                // local lambda
                double Square(double d) => Pow(d, 2);
                return PI * Square(Radius);
            }
        }

        public (double Circumference, double Area) Stats => (Circumference, Radius);

        //public double StatsIn ()   => (Circumference, Radius);


    }

    public class CircleUnitTest
    {
        private ITestOutputHelper _output;

        public CircleUnitTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void circle_demo()
        {
            // Arrange
            var circle = new Circle(1.0);

            // Act
            //var tu  = circle.Stats;
            var (circ, area) = circle.Stats;

            //c.Radius = 3.0; // :-( is RO
            this._output.WriteLine($@"radius : {circle.Radius}  ; {circle.Circumference} ; {circle.Area} ;" );

            this._output.WriteLine($@"cir: {circ} ; area : {area}");

            // Assert
            Assert.True(true);
        }
    }

}