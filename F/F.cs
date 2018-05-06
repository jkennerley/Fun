using System;
using System.Data;
using System.Data.SqlClient;

using static Ef.F;

namespace Ef
{
    public static partial class F
    {
        /// <summary>
        /// call a function-hof, and passing it a disposable, the disposable is 'used{}'  before it passed to the hof as a argument
        /// Using() is now an expression,  but using is a statement
        //// So => can use expression bodied method syntax and since expression has a value, Using() can be composed with other funcs
        /// </summary>
        /// <typeparam name="TDisp"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="disposable"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TR Using<TDisp, TR>(
            TDisp disposable,
            Func<TDisp, TR> func
        ) 
        where TDisp : IDisposable
        {
            using (var disp = disposable)
            {
                return func(disp);
            }

        }
    }
}

namespace Ef
{
    public static class ConnectionHelper
    {
        /// <summary>
        /// execute a function passing it a IDbConnection. This function will also wrap the call in a using(...){ ... } AND  calls open for you.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="connString"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static R Connect<R>(string connectionString , Func<IDbConnection, R> func) => Using(new SqlConnection(connectionString), conn => { conn.Open(); return func(conn); });
    }
}
