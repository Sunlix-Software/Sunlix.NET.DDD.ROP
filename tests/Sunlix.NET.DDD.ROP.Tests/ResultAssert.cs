namespace Sunlix.NET.DDD.ROP.Tests
{
    public static class ResultAssert
    {
        public static void Success<TSuccess, TFailure>(Result<TSuccess, TFailure> result, TSuccess expectedValue)
        {
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Value.Should().Be(expectedValue);
            result.Invoking(r => r.Error).Should().Throw<InvalidOperationException>();
        }

        public static void Failure<TSuccess, TFailure>(Result<TSuccess, TFailure> result, TFailure expectedError)
        {
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(expectedError);
            result.Invoking(r => r.Value).Should().Throw<InvalidOperationException>();
        }
    }
}
