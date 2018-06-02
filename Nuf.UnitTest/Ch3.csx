#r "C:\Users\jkennerley\jk\nufinv\Nuf\packages\xunit.extensibility.core.2.3.1\lib\netstandard1.1\xunit.core.dll"

// #r ".\Newtonsoft.Json.7.0.1\lib\net45\Newtonsoft.Json.dll"
// Environment.CurrentDirectory = "C:\\Users\\jkennerley\\jk\\nufinv\\Nuf\\Nuf.UnitTest"

public class Age
{
    public int Value { get; }
    public Age(int value)
    {
        if (!IsValid(value)) throw new ArgumentException($"{value} is not valid age");
        this.Value = value;
    }
    private static bool IsValid(int age) => 0 <= age && age < 120;
    public static bool operator <(Age a, Age r) => a.Value < r.Value;
    public static bool operator >(Age a, Age r) => a.Value > r.Value;
    public static bool operator <(Age a, int r) => a.Value < r;
    public static bool operator >(Age a, int r) => a.Value > r;
}

//Risk CalculateRiskProfile(Age age)
//{
//    return (age < 60) ? Risk.Low : Risk.Medium;
//}

public enum Gender { Female, Male };

Risk CalculateRiskProfile(Age age, Gender gender)
{
    var threshold = (gender == Gender.Female) ? 62 : 60;
    return (age < threshold) ? Risk.Low : Risk.Medium;
}

public class HealtData
{
    public Age Age;
    public Gender Gender;
}