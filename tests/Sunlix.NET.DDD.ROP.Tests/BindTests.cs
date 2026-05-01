using Sunlix.NET.DDD.BaseTypes;
using Sunlix.NET.DDD.ROP.Extensions;
using static Sunlix.NET.DDD.ROP.Tests.Traits;

namespace Sunlix.NET.DDD.ROP.Tests
{
    [Trait(Category, BindCategory)]
    public class BindTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public void Should_throw_exception_when_bind_function_is_null()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Result<Unit, Error>> bindFunction = null!;

            sut.Invoking(res => res.Bind(bindFunction))
                .Should()
                .Throw<ArgumentNullException>()
                .WithParameterName(nameof(bindFunction));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_rethrow_bind_function_exception()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            var exception = new InvalidOperationException();
            Func<Unit, Result<Unit, Error>> bindFunction = _ => throw exception;

            sut.Invoking(res => res.Bind(bindFunction))
                .Should()
                .Throw<InvalidOperationException>()
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_return_bind_function_result()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Result<int, Error>> bindFunction = 
                _ => Result.Succeed<int, Error>(0);

            var result = sut.Bind(bindFunction);

            ResultAssert.Success(result, 0);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_return_bind_function_failure()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Result<Unit, Error>> bindFunction =
                _ => Result.Fail<Unit, Error>(Errors.Error1);

            var result = sut.Bind(bindFunction);

            ResultAssert.Failure(result, Errors.Error1);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_return_initial_failure()
        {
            Result<Unit, Error> sut = UnitResult.Fail(Errors.Error1);
            Func<Unit, Result<Unit, Error>> bindFunction =
                _ => Result.Fail<Unit, Error>(Errors.Error2);

            var result = sut.Bind(bindFunction);

            ResultAssert.Failure(result, Errors.Error1);
        }
    }

    [Trait(Category, BindAsyncCategory)]
    public class BindAsyncTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_bind_function_is_null()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Task<Result<Unit, Error>>> bindFunctionAsync = null!;

            await sut.Invoking(res => res.BindAsync(bindFunctionAsync))
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(bindFunctionAsync));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_rethrow_bind_function_exception()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            var exception = new InvalidOperationException();
            Func<Unit, Task<Result<Unit, Error>>> bindFunctionAsync = 
                _ => Task.FromException<Result<Unit, Error>>(exception);

            (await sut.Invoking(res => res.BindAsync(bindFunctionAsync))
                .Should()
                .ThrowAsync<InvalidOperationException>())
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_bind_function_result()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Task<Result<int, Error>>> bindFunctionAsync =
                _ => Task.FromResult(Result.Succeed<int, Error>(0));

            var result = await sut.BindAsync(bindFunctionAsync);

            ResultAssert.Success(result, 0);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_bind_function_failure()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Task<Result<Unit, Error>>> bindFunctionAsync =
                _ => Task.FromResult(Result.Fail<Unit, Error>(Errors.Error1));

            var result = await sut.BindAsync(bindFunctionAsync);

            ResultAssert.Failure(result, Errors.Error1);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_initial_failure()
        {
            Result<Unit, Error> sut = UnitResult.Fail(Errors.Error1);
            Func<Unit, Task<Result<Unit, Error>>> bindFunctionAsync =
                _ => Task.FromResult(Result.Fail<Unit, Error>(Errors.Error2));

            var result = await sut.BindAsync(bindFunctionAsync);

            ResultAssert.Failure(result, Errors.Error1);
        }
    }

    [Trait(Category, ExtensionsBindCategory)]
    public class ExtensionsBindTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_bind_function_is_null()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Result<Unit, Error>> bindFunction = null!;

            await sut.Invoking(res => res.Bind(bindFunction))
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(bindFunction));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_rethrow_bind_function_exception()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            var exception = new InvalidOperationException();
            Func<Unit, Result<Unit, Error>> bindFunction = _ => throw exception;

            (await sut.Invoking(res => res.Bind(bindFunction))
                .Should()
                .ThrowAsync<InvalidOperationException>())
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_bind_function_result()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Result<int, Error>> bindFunction =
                _ => Result.Succeed<int, Error>(0);

            var result = await sut.Bind(bindFunction);

            ResultAssert.Success(result, 0);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_bind_function_failure()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Result<Unit, Error>> bindFunction =
                _ => Result.Fail<Unit, Error>(Errors.Error1);

            var result = await sut.Bind(bindFunction);

            ResultAssert.Failure(result, Errors.Error1);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_initial_failure()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Fail(Errors.Error1).AsTask();
            Func<Unit, Result<Unit, Error>> bindFunction =
                _ => Result.Fail<Unit, Error>(Errors.Error2);

            var result = await sut.Bind(bindFunction);

            ResultAssert.Failure(result, Errors.Error1);
        }
    }

    [Trait(Category, ExtensionsBindAsyncCategory)]
    public class ExtensionsBindAsyncTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_bind_function_is_null()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Task<Result<Unit, Error>>> bindFunctionAsync = null!;

            await sut.Invoking(res => res.BindAsync(bindFunctionAsync))
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(bindFunctionAsync));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_rethrow_bind_function_exception()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            var exception = new InvalidOperationException();
            Func<Unit, Task<Result<Unit, Error>>> bindFunctionAsync = 
                _ => Task.FromException<Result<Unit, Error>>(exception);

            (await sut.Invoking(res => res.BindAsync(bindFunctionAsync))
                .Should()
                .ThrowAsync<InvalidOperationException>())
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_bind_function_result()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Task<Result<int, Error>>> bindFunctionAsync =
                _ => Task.FromResult(Result.Succeed<int, Error>(0));

            var result = await sut.BindAsync(bindFunctionAsync);

            ResultAssert.Success(result, 0);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_bind_function_failure()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Task<Result<Unit, Error>>> bindFunctionAsync =
                _ => Task.FromResult(Result.Fail<Unit, Error>(Errors.Error1));

            var result = await sut.BindAsync(bindFunctionAsync);

            ResultAssert.Failure(result, Errors.Error1);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_initial_failure()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Fail(Errors.Error1).AsTask();
            Func<Unit, Task<Result<Unit, Error>>> bindFunctionAsync =
                _ => Task.FromResult(Result.Fail<Unit, Error>(Errors.Error2));

            var result = await sut.BindAsync(bindFunctionAsync);

            ResultAssert.Failure(result, Errors.Error1);
        }
    }
}
