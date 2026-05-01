using Sunlix.NET.DDD.BaseTypes;

using static Sunlix.NET.DDD.ROP.Tests.Traits;

namespace Sunlix.NET.DDD.ROP.Tests
{
    [Trait(Category, BindCategory)]
    public class BindTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_success_function_is_null()
        {
            Result<int, Error> Parse(string input) =>
                int.TryParse(input, out var value)
                ? Result.Succeed<int, Error>(value)
                : Result.Fail<int, Error>(new Error("ERR.01", "Invalid input"));

            var result = Parse("42")
              .Bind(x => x > 0 
              ? Result.Succeed<int, Error>(x * 2) 
              : Result.Fail<int, Error>(new Error("ERR.01", "Must be positive")));
            Console.WriteLine();

            async Task<Result<User, Error>> GetUserAsync(int id)
            {
                return Result.Succeed<User, Error>(new User { IsActive = false });
            }

            async Task<Result<User, Error>> EnsureActiveAsync(User user)
            {
                return user.IsActive
                      ? Result.Succeed<User, Error>(user)
                      : Result.Fail<User, Error>(new Error("ERR_01", "User is inactive"));
            }

            var result2 = await GetUserAsync(42)
              .BindAsync(EnsureActiveAsync);
            Console.WriteLine();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}
