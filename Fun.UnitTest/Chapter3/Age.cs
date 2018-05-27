using Fun;
using System;
using static Fun.F;

public class Age
{
    public int Value { get; }

    public Age(int age_)
    {
        if (!IsValid(age_))
        {
            throw new ArgumentException($@"{age_} is invalid age");
        }
        this.Value = age_;
    }

    private static bool IsValid(int age)
    {
        if (age >= 0 && age <= 120)
            return true;

        return false;
    }

    public static Option<Age> Of(int age) =>
        (IsValid(age)) ? Some(new Age(age)) : None;
}

//public struct Age
//{
//    //private int Value { get; }
//
//    //private Age(int value)
//    //{
//    //    if (!IsValid(value))
//    //        throw new ArgumentException($"{value} is not a valid age");
//    //
//    //    Value = value;
//    //}
//
//    //private static bool IsValid(int age)
//    //    => 0 <= age && age < 120;
//    //
//    //public static Option<Age> Of(int age)
//    //    => IsValid(age) ? Some(new Age(age)) : None;
//    //
//    //public static bool operator <(Age l, Age r) => l.Value < r.Value;
//    //public static bool operator >(Age l, Age r) => l.Value > r.Value;
//    //
//    //public static bool operator <(Age l, int r) => l < new Age(r);
//    //public static bool operator >(Age l, int r) => l > new Age(r);
//    //
//    //public override string ToString() => Value.ToString();
//}