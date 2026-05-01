using Sunlix.NET.DDD.BaseTypes;
using Sunlix.NET.DDD.ROP.Extensions;
using static Sunlix.NET.DDD.ROP.Tests.Traits;

namespace Sunlix.NET.DDD.ROP.Tests
{
    [Trait(Category, TeeCategory)]
    public class TeeTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public void Should_throw_exception_when_side_effect_is_null()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Action<Unit> sideEffect = null!;

            sut.Invoking(res => res.Tee(sideEffect))
                .Should()
                .Throw<ArgumentNullException>()
                .WithParameterName(nameof(sideEffect));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_rethrow_side_effect_exception()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            var exception = new InvalidOperationException();
            Action<Unit> sideEffect = _ => throw exception;

            sut.Invoking(res => res.Tee(sideEffect))
                .Should()
                .Throw<InvalidOperationException>()
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_execute_side_effect_on_success()
        {
            Result<int, Error> sut = Result.Succeed<int, Error>(0);
            var sideEffectExecuted = false;

            Action<int> sideEffect = value => sideEffectExecuted = true;

            var result = sut.Tee(sideEffect);

            sideEffectExecuted.Should().BeTrue();
            ResultAssert.Success(result, 0);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_not_execute_side_effect_on_failure()
        {
            Result<Unit, int> sut = UnitResult.Fail(1);
            var sideEffectExecuted = false;

            Action<Unit> sideEffect = _ => sideEffectExecuted = true;

            var result = sut.Tee(sideEffect);

            sideEffectExecuted.Should().BeFalse();
            ResultAssert.Failure(result, 1);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_return_same_result_instance()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Action<Unit> sideEffect = _ => { };

            var result = sut.Tee(sideEffect);

            result.Should().BeSameAs(sut);
        }
    }

    [Trait(Category, TeeAsyncCategory)]
    public class TeeAsyncTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_side_effect_is_null()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Task> sideEffectAsync = null!;

            await sut.Invoking(res => res.TeeAsync(sideEffectAsync))
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(sideEffectAsync));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_rethrow_side_effect_exception()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            var exception = new InvalidOperationException();
            Func<Unit, Task> sideEffectAsync = _ => Task.FromException(exception);

            (await sut.Invoking(res => res.TeeAsync(sideEffectAsync))
                .Should()
                .ThrowAsync<InvalidOperationException>())
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_execute_side_effect_on_success()
        {
            Result<int, Error> sut = Result.Succeed<int, Error>(0);
            var sideEffectExecuted = false;

            Func<int, Task> sideEffectAsync = _ =>
            {
                sideEffectExecuted = true;
                return Task.CompletedTask;
            };

            var result = await sut.TeeAsync(sideEffectAsync);

            sideEffectExecuted.Should().BeTrue();
            ResultAssert.Success(result, 0);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_not_execute_side_effect_on_failure()
        {
            Result<Unit, int> sut = UnitResult.Fail(1);
            var sideEffectExecuted = false;

            Func<Unit, Task> sideEffectAsync = _ =>
            {
                sideEffectExecuted = true;
                return Task.CompletedTask;
            };

            var result = await sut.TeeAsync(sideEffectAsync);

            sideEffectExecuted.Should().BeFalse();
            ResultAssert.Failure(result, 1);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_same_result_instance()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            Func<Unit, Task> sideEffectAsync = _ => Task.CompletedTask;

            var result = await sut.TeeAsync(sideEffectAsync);

            result.Should().BeSameAs(sut);
        }
    }

    [Trait(Category, ExtensionsTeeCategory)]
    public class ExtensionsTeeTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_side_effect_is_null()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Action<Unit> sideEffect = null!;

            await sut.Invoking(res => res.Tee(sideEffect))
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(sideEffect));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_rethrow_side_effect_exception()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            var exception = new InvalidOperationException();
            Action<Unit> sideEffect = _ => throw exception;

            (await sut.Invoking(res => res.Tee(sideEffect))
                .Should()
                .ThrowAsync<InvalidOperationException>())
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_execute_side_effect_on_success()
        {
            Task<Result<int, Error>> sut = Task.FromResult(Result.Succeed<int, Error>(0));
            var sideEffectExecuted = false;

            Action<int> sideEffect = value => sideEffectExecuted = true;

            var result = await sut.Tee(sideEffect);

            sideEffectExecuted.Should().BeTrue();
            ResultAssert.Success(result, 0);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_not_execute_side_effect_on_failure()
        {
            Task<Result<Unit, int>> sut = UnitResult.Fail(1).AsTask();
            var sideEffectExecuted = false;

            Action<Unit> sideEffect = _ => sideEffectExecuted = true;

            var result = await sut.Tee(sideEffect);

            sideEffectExecuted.Should().BeFalse();
            ResultAssert.Failure(result, 1);
        }
    }

    [Trait(Category, ExtensionsTeeAsyncCategory)]
    public class ExtensionsTeeAsyncTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_side_effect_is_null()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            Func<Unit, Task> sideEffectAsync = null!;

            await sut.Invoking(res => res.TeeAsync(sideEffectAsync))
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(sideEffectAsync));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_rethrow_side_effect_exception()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            var exception = new InvalidOperationException();
            Func<Unit, Task> sideEffectAsync = _ => Task.FromException(exception);

            (await sut.Invoking(res => res.TeeAsync(sideEffectAsync))
                .Should()
                .ThrowAsync<InvalidOperationException>())
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_execute_side_effect_on_success()
        {
            Task<Result<int, Error>> sut = Task.FromResult(Result.Succeed<int, Error>(0));
            var sideEffectExecuted = false;

            Func<int, Task> sideEffectAsync = _ =>
            {
                sideEffectExecuted = true;
                return Task.CompletedTask;
            };

            var result = await sut.TeeAsync(sideEffectAsync);

            sideEffectExecuted.Should().BeTrue();
            ResultAssert.Success(result, 0);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_not_execute_side_effect_on_failure()
        {
            Task<Result<Unit, int>> sut = UnitResult.Fail(1).AsTask();
            var sideEffectExecuted = false;

            Func<Unit, Task> sideEffectAsync = _ =>
            {
                sideEffectExecuted = true;
                return Task.CompletedTask;
            };

            var result = await sut.TeeAsync(sideEffectAsync);

            sideEffectExecuted.Should().BeFalse();
            ResultAssert.Failure(result, 1);
        }
    }
}
