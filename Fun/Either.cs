using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;

namespace Fun
{
    // Left & Right structs
    public static class Either
    {
        public struct Left<L>
        {
            internal L Value { get; }

            internal Left(L value)
            {
                Value = value;
            }

            public override string ToString() => $@"Left({Value})";
        }

        public struct Right<R>
        {
            internal R Value { get; }

            internal Right(R value)
            {
                Value = value;
            }

            public override string ToString() => $@"Right({Value})";

            //////////////////public Right<RR> Map<L, RR>(Func<R, RR> f) => Right(f(Value));
            //////////////////public Either<L, RR> Bind<L, RR>(Func<R, Either<L, RR>> f) => f(Value);
        }
    }
}

namespace Fun
{
    // Create of Left & Right structs
    public static partial class Create
    {
        public static Either.Left<L> Left<L>(L l) => new Either.Left<L>(l);

        public static Either.Right<R> Right<R>(R r) => new Either.Right<R>(r);
    }

    //public static partial class F
    //{
    //    public static Either.Left<L> Left<L>(L l) => Create.Left<L>(l);
    //
    //    public static Either.Right<R> Right<R>(R r) => Create.Right<R>(r);
    //}
}


namespace Fun
{
    // Either<L,R> struct
    public struct Either<L, R>
    {
        internal L Left { get; }

        internal R Right { get; }

        public bool IsRight { get; }

        //public bool IsLeft => !IsRight;

        internal Either(L left)
        {
            IsRight = false;
            Left = left;
            Right = default(R);
        }

        internal Either(R right)
        {
            IsRight = true;
            Right = right;
            Left = default(L);
        }


        // copy constructor from lifted Either.Left.Value e.g.  Either<string, double> either = "oops";
        public static implicit operator Either<L, R>(L left) => new Either<L, R>(left);

        // copy constructor from lifted Either.Right value e.g.  Either<string, double> either = 12.0 ;
        public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);

        // copy constructor from Either.Left e.g. Either<string, double> either = Left("oops");
        public static implicit operator Either<L, R>(Either.Left<L> left) => new Either<L, R>(left.Value);

        // copy constructor from Either.Right.Value e.g. 
        public static implicit operator Either<L, R>(Either.Right<R> right) => new Either<L, R>(right.Value);

        // Match, calls the left-fun() if the either is in the left state, or calls the right-fun() if in the right state
        public TR Match<TR>(Func<L, TR> leftFun, Func<R, TR> rightFun)
            => !this.IsRight? leftFun(this.Left) : rightFun(this.Right);

        //public Unit Match(Action<L> Left, Action<R> Right) => Match(Left.ToFunc(), Right.ToFunc());

        //public IEnumerator<R> AsEnumerable()
        //{
        //    if (IsRight) yield return Right;
        //}

        //public override string ToString() => Match(l => $@"Left({l})", r => $"Right({r})");

    }
}