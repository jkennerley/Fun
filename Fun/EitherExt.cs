using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;


//using Fun.F;
using static Fun.Create;

using static Fun.F;

namespace Fun
{
    using static F;

    public static class EitherExt
    {
        /// <summary>
        /// Map : 
        /// if the either (the 'this' ) is 
        ///   in the left-sad state, then return a new Either in the left state
        ///   ELSE 
        ///     is in the right-happy state, then return a new Either in the right state, 
        ///     and the right-happy-type of the either is the returning-type of the mapping-function 
        /// </summary>
        /// <typeparam name="L"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <typeparam name="RR"></typeparam>
        /// <param name="this"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Either<L, RR> Map<L, R, RR>
           (this Either<L, R> @thisEither, Func<R, RR> mappingFun)
           => @thisEither.Match<Either<L, RR>>(
              l => Left(l),
              r => (mappingFun(r)));
        

        //public static Either<LL, RR> Map<L, LL, R, RR>
        //   (this Either<L, R> @this, Func<L, LL> left, Func<R, RR> right)
        //   => @this.Match<Either<LL, RR>>(
        //      l => Left(left(l)),
        //      r => Right(right(r)));
        

        //public static Either<L, Unit> ForEach<L, R>
        //   (this Either<L, R> @this, Action<R> act)
        //   => Map(@this, act.ToFunc());
        //

        public static Either<L, RR> Bind<L, R, RR>
           (this Either<L, R> @this, Func<R, Either<L, RR>> f)
           => @this.Match(
              l => Left(l),
              r => f(r));
    
    
        // LINQ
    
        //public static Either<L, R> Select<L, T, R>(this Either<L, T> @this
        //   , Func<T, R> map) => @this.Map(map);
    
    
       //public static Either<L, RR> SelectMany<L, T, R, RR>(this Either<L, T> @this
        //   , Func<T, Either<L, R>> bind, Func<T, R, RR> project)
        //   => @this.Match(
        //      Left: l => Left(l),
        //      Right: t =>
        //         bind(@this.Right).Match<Either<L, RR>>(
        //            Left: l => Left(l),
        //            Right: r => project(t, r)));
    }
}


//public static class EitherExt
//{
//    public static Either<L, RR> Map<L, R, RR>
//       (this Either<L, R> @this, Func<R, RR> f)
//       => @this.Match<Either<L, RR>>(
//          l => Left(l),
//          r => Right(f(r)));
//
//    public static Either<LL, RR> Map<L, LL, R, RR>
//       (this Either<L, R> @this, Func<L, LL> left, Func<R, RR> right)
//       => @this.Match<Either<LL, RR>>(
//          l => Left(left(l)),
//          r => Right(right(r)));
//
//    public static Either<L, Unit> ForEach<L, R>
//       (this Either<L, R> @this, Action<R> act)
//       => Map(@this, act.ToFunc());
//
//    public static Either<L, RR> Bind<L, R, RR>
//       (this Either<L, R> @this, Func<R, Either<L, RR>> f)
//       => @this.Match(
//          l => Left(l),
//          r => f(r));
//
//
//    // LINQ
//
//    public static Either<L, R> Select<L, T, R>(this Either<L, T> @this
//       , Func<T, R> map) => @this.Map(map);
//
//
//    public static Either<L, RR> SelectMany<L, T, R, RR>(this Either<L, T> @this
//       , Func<T, Either<L, R>> bind, Func<T, R, RR> project)
//       => @this.Match(
//          Left: l => Left(l),
//          Right: t =>
//             bind(@this.Right).Match<Either<L, RR>>(
//                Left: l => Left(l),
//                Right: r => project(t, r)));
//}
