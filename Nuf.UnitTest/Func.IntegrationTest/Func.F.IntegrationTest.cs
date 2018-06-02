using Xunit;
using Xunit.Abstractions;

namespace Func.F
{
    public class FIntgrationTest
    {
        private ITestOutputHelper _output { get; }

        public FIntgrationTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        public class AspNetUser
        {
            public string UserName { get; set; }
        }

        [Fact]
        public void Connect_should_get_data_for_simple_sql_select()
        {
            //// Arrange
            //var cns = "server=.\\SQLExpress ; database=TotalAoc - Client Copy; Trusted_Connection=True";
            //
            //var xs =
            //    Connect(cns, cn => cn.Query<AspNetUser>("select * from aspnet_Users"))
            //    .ToList();
            //
            //// Act
            //
            //// Assert
            //Assert.NotNull(xs);
            //Assert.True(xs.ToList().Count > 0);
            //this._output.WriteLine($@" {  xs[0].UserName   }");
        }
    }
}