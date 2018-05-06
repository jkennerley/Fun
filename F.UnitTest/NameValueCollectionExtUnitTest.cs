using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

using Ef;
using static Ef.F;
using static Ef.NameValueCollectionExt;

namespace DictionaryExtUnitTest
{

    public class NameValueCollectionExtUnitTest
    {
        private readonly ITestOutputHelper output;

        public NameValueCollectionExtUnitTest(ITestOutputHelper output_)
        {
            this.output = output_;
        }

        [Fact]
        public void NameValueCollectionExt_lookup_to_OptionT()
        {
            // Arrange
            var xs = new NameValueCollection();

            xs.Add("a", "AA");
            xs.Add("b", "BB");

            // Act

            // Assert

            // the usual dictionary stuff
            Assert.Equal("AA" , xs["a"]);
            Assert.Equal("BB" , xs["b"]);

            // 
            Assert.Equal(  None, xs.Lookup("-NOT-THERE"));
            Assert.Equal(  Some("AA"), xs.Lookup("a"));

            var actualA = xs.Lookup("a").Match( () => "" , v => v);
            Assert.Equal(actualA, "AA");

            var actualANotExist = xs.Lookup("a-key-there-").Match(() => "", v => v);
            Assert.Equal("" , actualANotExist);

        }

    }
}