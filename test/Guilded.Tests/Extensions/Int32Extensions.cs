using Shouldly;
using System.Net;

namespace Guilded.Tests.Extensions
{
    public static class Int32Extensions
    {
        public static void ShouldBe(this int value, HttpStatusCode statusCode) => value.ShouldBe((int)statusCode);
        public static void ShouldBe(this int? value, HttpStatusCode statusCode) => value.ShouldBe((int)statusCode);
    }
}
