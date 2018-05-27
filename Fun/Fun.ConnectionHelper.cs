using System;
using System.Data;
using System.Data.SqlClient;

using static Fun.F;

namespace Fun
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
        public static R Connect<R>(string connectionString, Func<IDbConnection, R> func) => Using(new SqlConnection(connectionString), conn => { conn.Open(); return func(conn); });
    }
}