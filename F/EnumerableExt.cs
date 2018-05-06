using System;
using System.Collections.Generic;
using System.Linq;

namespace Ef
{
    using static F;

    public static class EnumerableExt
    {
        public static IEnumerable<R> Map<T, R>(
            this IEnumerable<T> ts,
            Func<T, R> f
        )
        {
            return ts.Select(f);
        }

    }
}