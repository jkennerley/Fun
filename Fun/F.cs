using System;

using Unit = System.ValueTuple;

namespace Fun
{
    public static partial class F
    {
        public static Unit Unit() => default(Unit);

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
