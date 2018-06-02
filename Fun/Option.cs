using System;
using System.Collections.Generic;

using Unit = System.ValueTuple;

namespace Fun
{
    using static F;

    public static partial class F
    {
        // wrap the given value into a Some
        public static Option<T> Some<T>(T value) => new Option.Some<T>(value);

        // the None value
        public static Option.None None => Option.None.Default;
    }

    public struct Option<T> : IEquatable<Option.None>, IEquatable<Option<T>>
    {
        private readonly T value;
        private readonly bool isSome;
        bool isNone => !isSome;

        private Option(T value)
        {
            if (value == null)
                throw new ArgumentNullException();
            this.isSome = true;
            this.value = value;
        }

        public static implicit operator Option<T>(Option.None _) => new Option<T>();

        public static implicit operator Option<T>(Option.Some<T> some) => new Option<T>(some.Value);

        public static implicit operator Option<T>(T value)
           => value == null ? None : Some(value);

        public R Match<R>(Func<R> None, Func<T, R> Some) => isSome ? Some(value) : None();

        public IEnumerable<T> AsEnumerable()
        {
            if (isSome) yield return value;
        }

        public bool Equals(Option<T> other)
           => this.isSome == other.isSome
           && (this.isNone || this.value.Equals(other.value));

        public bool Equals(Option.None _) => isNone;

        public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);

        public static bool operator !=(Option<T> @this, Option<T> other) => !(@this == other);

        public override string ToString() => isSome ? $"Some({value})" : "None";
    }

    namespace Option
    {
        public struct None
        {
            internal static readonly None Default = new None();
        }

        public struct Some<T>
        {
            //internal T Value { get; }
            public T Value { get; }

            internal Some(T value)
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value)
                       , "Cannot wrap a null value in a 'Some'; use 'None' instead");
                Value = value;
            }
        }
    }

    public static class OptionExt
    {
        /// <summary>
        /// returns the inner value of the Option if any and blows if None
        /// </summary>
        public static T Value<T>
            (this Option<T> optT)
            => optT.Match(
                () => throw new ArgumentException("Cannot return value because the value was None"),
                (t) => t);

        public static Option<R> Bind<T, R>
           (this Option<T> optT, Func<T, Option<R>> f)
            => optT.Match(
               () => None,
               (t) => f(t));

        public static Option<Unit> ForEach<T>(this Option<T> @this, Action<T> action)
           => Map(@this, action.ToFunc());

        // lifts fun returning string to Option<String>
        public static Option<R> Map<T, R>
           (this Option<T> optT, Func<T, R> f)
           => optT.Match(
              () => None,
              (t) => Some(f(t)));

        // for an Option, does a predicate Hit, if it does return that value in an option ; filter for where the predicate hits
        public static Option<T> Where<T>
           (this Option<T> optT, Func<T, bool> predicate)
           => optT.Match(
              () => None,
              (t) => predicate(t) ? optT : None);
    }
}
