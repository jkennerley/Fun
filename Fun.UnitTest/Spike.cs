using System;
using Ef;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Fun;
using Xunit;
using Xunit.Abstractions;
using static Fun.F;
using static Fun.String;

//using static Ef.Option;

namespace F.UnitTest
{
    public class SpikeUnitTest
    {
        private ITestOutputHelper output;

        public SpikeUnitTest(ITestOutputHelper output_)
        {
            this.output = output_;
        }

        [Fact]
        public void ForEach_for_side_effect_over_Enumerable()
        {
            // Arrange

            // List has the ForEach extension method
            var xs = new List<int> { 1, 2, 3 };

            //  an Enumerable
            var xse = xs.AsEnumerable();

            // Action<int> WriteLine = (x) => output.WriteLine($@"{x}");
            void WriteLine(int x) => output.WriteLine($@"{x}");

            // Act

            // ForEach
            xs.ForEach(WriteLine);

            // there is no ForEach that takes an action on IEnumerable, until you add an extension method
            xse.ForEach(WriteLine);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void ForEach_for_side_effect()
        {
            // Arrange
            var xs = new List<int> { 1, 2, 3 };
            var xse = xs.AsEnumerable();

            // IE of names
            var names = new[] { "Adam", "Ben", "Cheryl" }.AsEnumerable();

            // option array of names
            Option<string>[] nameOptions = { "Adam" };

            void WriteItem(int x) => output.WriteLine($@"{x}");

            // Act

            xs.ForEach(WriteItem);

            xse.ForEach(WriteItem);

            names
                .Map(ToUpper)// Ef.String.ToUpper
                .ForEach(output.WriteLine);

            //nameOptions
            //    .Map(ToUpper)// Ef.String.ToUpper
            //    .ForEach(output.WriteLine);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void Age_of_smart_constructor()
        {
            // Arrange
            //var i_ = Int.Parse("1");

            var ageMinus1_ = Age.Of(-1);

            var age1_ = Age.Of(1);

            // Act

            // Assert

            Assert.Equal(None, ageMinus1_);

            var ex = age1_.Match(() => null, x => x);
            Assert.Equal(1, ex.Value);
        }

        [Fact]
        public void using_map_function_to_take_string_to_int_to_age_to_which_is_option_of_option()
        {
            // Arrange
            var i_ = Int.Parse("1");
            var age_ = i_.Map(i => Age.Of(i));

            Option<int> iOpt = Int.Parse("1");

            // map  wraps the option in an option?
            Option<Option<Age>> ageOptOpt = iOpt.Map(i => Age.Of(i));

            // Act

            // Assert
            var acOpt = ageOptOpt = ageOptOpt.Match(() => null, t => t);
            // ...
        }

        [Fact]
        public void using_bind_function_to_take_string_to_int_to_age_to_which_is_option_of_option()
        {
            // Arrange
            var i = Int.Parse("1");
            //...var age = i_.Map(i => Age.Of(i));

            var i_ = Int.Parse("1");

            // Bind : age is Option<Age>
            // i_ -> age_ ->
            //   so   1 -> Age(1)
            //   so -1  -> None
            //   so 160 -> None
            var age = i_.Bind(Age.Of);

            // map  wraps the option in an option?
            //Option<Age> ageOpt = iOpt.Bind(i => Age.Of(i));

            // Act

            // Assert
            var ac = Int.Parse("1").Bind(Age.Of).Value(); // should not really use the value function
            Assert.Equal(1, ac.Value);
        }

        [Fact]
        public void using_bind_to_parse_age()
        {
            // Arrange

            //Func<string, Option<Age>> parseAge = s => Int.Parse(s).Bind(Age.Of);
            Option<Age> parseAge(string s) => Int.Parse(s).Bind(Age.Of);

            // Act

            // Assert
            Assert.Equal(None, parseAge("notAnAge"));
            Assert.Equal(None, parseAge("180"));
            Assert.Equal(None, parseAge("-1"));

            //var ac1 = parseAge("1").Value().Value;
            var age1 = parseAge("1").Value();

            Assert.Equal(1, parseAge("1").Value().Value);
        }

        internal class Pet
        {
            private readonly string name;

            private Pet(string name)
            {
                this.name = name;
            }

            public static implicit operator Pet(string name)
                => new Pet(name);
        }

        [Fact]
        public void S431_Bind_and_IEnumerable_SelectMany()
        {
            var neighbours = new[]
            {
                new {Name="Adam 1" , Pets = new Pet[]{"Fluffy" ,"Thor"}  },
                new {Name="Beryl 3" , Pets = new Pet[]{}  },
                new {Name="Charlie 2" , Pets = new Pet[]{"Sybill" }  }
            };

            // IE of Pet arrays
            var bucketsOfpets = neighbours.Map(x => x.Pets);

            // IE of Pet
            var pets = neighbours.Bind(n => n.Pets);

            pets.ToList().ForEach(x =>
            {
                this.output.WriteLine($@"{x}");
            });
        }

        [Fact]
        public void S44_Filter_values_with_where()
        {
            // Arrange
            bool IsNatural(int i) => i > 0;

            Option<int> ToNatural(string s) =>
                Int.Parse(s).Where(IsNatural);

            // Act

            // Assert
            Assert.Equal(None, ToNatural("nOT_A numb3r"));
            Assert.Equal(None, ToNatural("-1"));
            Assert.Equal(Some(1), ToNatural("1"));
        }

        [Fact]
        public void S45_Option_as_list_with_either_0_or_1_items()
        {
            // Arrange
            var n = Some("thing").AsEnumerable().Count();

            var v = Some("thing").AsEnumerable().FirstOrDefault();

            // Act

            // Assert
            Assert.Equal(n, 1);
            Assert.Equal("thing", v);
        }

        [Fact]
        public void S5_programs_with_function_composition()
        {
            string Abbreviate(string s) => s.Substring(0, 2).ToLower();
            string AbbreviationName(Person p_) => Abbreviate(p_.Forename) + Abbreviate(p_.Surname);
            string AppendDomain(string localPart) => $@"{localPart}@acme.com";

            string EmailFor(Person p) => AppendDomain(AbbreviationName(p));

            // Arrange
            var person = new Person { Surname = "Smith", Forename = "John" };

            // Act
            var email = EmailFor(person);

            // Assert
            Assert.Equal("josm@acme.com", email);
        }

        [Fact]
        public void S512_programs_with_function_composition_method_chaining()
        {
            // Arrange
            var person = new Person { Surname = "Smith", Forename = "John" };

            // Act
            var email = person.AbbreviationName().AppendDomain();

            // Assert
            Assert.Equal("josm@acme.com", email);
        }

        [Fact]
        public void S513_function_composition_in_elevated_world_with_Option_Person()
        {
            // Arrange

            // person option
            //var person_ = None ;
            var person_ = Some(new Person { Surname = "Smith", Forename = "John" });

            // emailFor : Person -> String
            string emailFor(Person p) => p.AbbreviationName().AppendDomain();

            // Act

            // apply regular function in the elevated world ;-)
            // call emailFor on the option wrapping the result in an option
            var email_ = person_.Map(emailFor);

            // Assert :
            var email = email_.Value();
            Assert.Equal("josm@acme.com", email_);
            Assert.Equal("josm@acme.com", email);
        }

        [Fact]
        public void S531_function_composition_in_elevated_world_with_Option_Person()
        {
            // Arrange

            // person option
            var xs = new List<Person>
            {
                new Person{Earnings = 1.0m},
                new Person{Earnings = 2.0m},
                new Person{Earnings = 3.0m},
                new Person{Earnings = 4.0m},
                new Person{Earnings = 5.0m},
                new Person{Earnings = 6.0m},
                new Person{Earnings = 7.0m},
                new Person{Earnings = 8.0m}
            };

            // Act
            var avgEarningsOfRichestQuartile =
                xs
                    .OrderByDescending(p => p.Earnings)
                    .Take(xs.Count / 4)
                    .Select(x => x.Earnings)
                    .Average();

            // Assert :
            Assert.Equal(7.5m, avgEarningsOfRichestQuartile);
        }

        [Fact]
        public void S522_functions_that_compos_well()
        {
            // Arrange

            // person option
            var population = 
                Enumerable.Range(1,8)
                .Select( x=>
                    new Person{Earnings = x}
                )
                .ToList()
                ;

            // Act
            var avgEarningsOfRichestQuartile =
                population
                .RichestQuartile()
                .averageEarnings();

            // Assert :
            Assert.Equal(7.5m, avgEarningsOfRichestQuartile);
        }

        [Fact]
        public void S53_programming_workflows()
        {
            // Arrange

            // person option
            //var population =
            //        Enumerable.Range(1, 8)
            //            .Select(x =>
            //                new Person { Earnings = x }
            //            )
            //            .ToList()
            //    ;
            
            // Act
            //var avgEarningsOfRichestQuartile =
            //    population
            //        .RichestQuartile()
            //        .averageEarnings();
            
            // Assert :
            //Assert.Equal(7.5m, avgEarningsOfRichestQuartile);
        }


        [Fact]
        public void S54_Modelling_FP_vs_OOP()
        {
            // Arrange
            var account = new AccountOOP(100);
            // Act
            account.Debit(50);
            // Assert
            Assert.Equal( 50 , account.Balance);
        }

    }


    public class AccountOOP
    {
        public decimal Balance { get; private set; }

        public AccountOOP( decimal  balance)
        {
            this.Balance = balance;
        }

        public void Debit(decimal amount)
        {
            if(Balance < amount )
                throw new InvalidOperationException("Insufficient funds");

            Balance -= amount;
        }
    }


    public class AccountState
    {
        public decimal Balance { get; }

        public AccountState(decimal balance)
        {
            this.Balance = balance;
        }
    }

    public static class AccountStateExt
    {
        //public static AccountState Debit( AccountState accountState, decimal  amount )
        //{
        //    return new AccountState(accountState.Balance - amount);
        //}

        public static Option<AccountState> Debit(AccountState acc, decimal amount)
        =>
                (acc.Balance < amount)
                ? None
                : Some(new AccountState(acc.Balance - amount));

    }





    public class Person
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public decimal Earnings { get; set; }
    }

    public static class PersonExt
    {
        public static string AbbreviationName(this Person p_) => Abbreviate(p_.Forename) + Abbreviate(p_.Surname);

        public static string Abbreviate(string s) => s.Substring(0, 2).ToLower();

        public static string AppendDomain(this string localPart) => $@"{localPart}@acme.com";

        public static IEnumerable<Person> RichestQuartile 
            (this List<Person> pop ) 
            => pop.OrderByDescending(p => p.Earnings).Take(pop.Count / 4);

        public static decimal averageEarnings (this IEnumerable<Person>  xs) =>
            xs.Select(x => x.Earnings).Average();



    }
}