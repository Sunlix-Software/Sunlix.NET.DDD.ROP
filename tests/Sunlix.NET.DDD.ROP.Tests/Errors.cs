using Sunlix.NET.DDD.BaseTypes;

namespace Sunlix.NET.DDD.ROP.Tests
{
    internal static class Errors
    {
        public static Error Error1 => new Error("error1", "Error1");
        public static Error Error2 => new Error("error2", "Error2");
    }
}
