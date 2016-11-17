using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Selama.Tests
{
    public class SampleUnitTest
    {
        [Fact]
        public void Passed()
        {
            Assert.True(true);
        }

        [Fact]
        public void Failed()
        {
            Assert.True(false);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Theory(int number)
        {
            Assert.True(IsOdd(number));
        }

        private bool IsOdd(int number)
        {
            return number % 2 == 1;
        }
    }
}
