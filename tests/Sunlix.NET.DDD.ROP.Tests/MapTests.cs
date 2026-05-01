using Sunlix.NET.DDD.BaseTypes;
using Sunlix.NET.DDD.ROP.Extensions;
using static Sunlix.NET.DDD.ROP.Tests.Traits;

namespace Sunlix.NET.DDD.ROP.Tests
{
    [Trait(Category, MapCategory)]
    public class MapTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public void Should_throw_exception_when_map_function_is_null()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, int> mapFunction = null!;

            sut.Invoking(res => res.Map(mapFunction))
                .Should()
                .Throw<ArgumentNullException>()
                .WithParameterName(nameof(mapFunction));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_rethrow_map_function_exception()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            var exception = new InvalidOperationException();
            Func<Unit, int> mapFunction = _ => throw exception;

            sut.Invoking(res => res.Map(mapFunction))
                .Should()
                .Throw<InvalidOperationException>()
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_return_map_function_result()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, int> mapFunction = _ => 0;

            var result = sut.Map(mapFunction);

            ResultAssert.Success(result, 0);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_return_initial_failure()
        {
            Result<Unit, int> sut = UnitResult.Fail(1);
            Func<Unit, int> mapFunction = _ => 0;

            var result = sut.Map(mapFunction);

            ResultAssert.Failure(result, 1);
        }
    }

    [Trait(Category, MapAsyncCategory)]
    public class MapAsyncTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_map_function_is_null()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Task<int>> mapFunctionAsync = null!;

            await sut.Invoking(res => res.MapAsync(mapFunctionAsync))
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(mapFunctionAsync));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_rethrow_map_function_exception()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            var exception = new InvalidOperationException();
            Func<Unit, Task<int>> mapFunctionAsync =
                _ => Task.FromException<int>(exception);

            (await sut.Invoking(res => res.MapAsync(mapFunctionAsync))
                .Should()
                .ThrowAsync<InvalidOperationException>())
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_map_function_result()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Task<int>> mapFunctionAsync = _ => Task.FromResult(0);

            var result = await sut.MapAsync(mapFunctionAsync);

            ResultAssert.Success(result, 0);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_initial_failure()
        {
            Result<Unit, int> sut = UnitResult.Fail(1);
            Func<Unit, Task<int>> mapFunctionAsync = _ => Task.FromResult(0);

            var result = await sut.MapAsync(mapFunctionAsync);

            ResultAssert.Failure(result, 1);
        }
    }

    [Trait(Category, ExtensionsMapCategory)]
    public class ExtensionsMapTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_map_function_is_null()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, int> mapFunction = null!;

            await sut.Invoking(res => res.Map(mapFunction))
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(mapFunction));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_rethrow_map_function_exception()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            var exception = new InvalidOperationException();
            Func<Unit, int> mapFunction = _ => throw exception;

            (await sut.Invoking(res => res.Map(mapFunction))
                .Should()
                .ThrowAsync<InvalidOperationException>())
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_map_function_result()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, int> mapFunction = _ => 0;

            var result = await sut.Map(mapFunction);

            ResultAssert.Success(result, 0);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_initial_failure()
        {
            Task<Result<Unit, int>> sut = UnitResult.Fail(1).AsTask();
            Func<Unit, int> mapFunction = _ => 0;

            var result = await sut.Map(mapFunction);

            ResultAssert.Failure(result, 1);
        }
    }

    [Trait(Category, ExtensionsBindAsyncCategory)]
    public class ExtensionsMapAsyncTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_map_function_is_null()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Task<int>> mapFunctionAsync = null!;

            await sut.Invoking(res => res.MapAsync(mapFunctionAsync))
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(mapFunctionAsync));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_rethrow_map_function_exception()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            var exception = new InvalidOperationException();
            Func<Unit, Task<int>> mapFunctionAsync =
                _ => Task.FromException<int>(exception);

            (await sut.Invoking(res => res.MapAsync(mapFunctionAsync))
                .Should()
                .ThrowAsync<InvalidOperationException>())
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_map_function_result()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Task<int>> mapFunctionAsync = _ => Task.FromResult(0);

            var result = await sut.MapAsync(mapFunctionAsync);

            ResultAssert.Success(result, 0);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_initial_failure()
        {
            Task<Result<Unit, int>> sut = UnitResult.Fail(1).AsTask();
            Func<Unit, Task<int>> mapFunctionAsync = _ => Task.FromResult(0);

            var result = await sut.MapAsync(mapFunctionAsync);

            ResultAssert.Failure(result, 1);
        }
    }
}
