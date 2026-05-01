using Sunlix.NET.DDD.BaseTypes;
using Sunlix.NET.DDD.ROP.Extensions;
using static Sunlix.NET.DDD.ROP.Tests.Traits;

namespace Sunlix.NET.DDD.ROP.Tests
{
    [Trait(Category, TeeErrorCategory)]
    public class TeeErrorTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public void Should_throw_exception_when_side_effect_is_null()
        {
            Result<Unit, Error> sut = UnitResult.Fail(Errors.Error1);
            Action<Error> sideEffect = null!;

            sut.Invoking(res => res.TeeError(sideEffect))
                .Should()
                .Throw<ArgumentNullException>()
                .WithParameterName(nameof(sideEffect));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_rethrow_side_effect_exception()
        {
            Result<Unit, Error> sut = UnitResult.Fail(Errors.Error1);
            var exception = new InvalidOperationException();
            Action<Error> sideEffect = _ => throw exception;

            sut.Invoking(res => res.TeeError(sideEffect))
                .Should()
                .Throw<InvalidOperationException>()
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_execute_side_effect_on_failure()
        {
            var error = Errors.Error1;
            Result<Unit, Error> sut = Result.Fail<Unit, Error>(error);
            var sideEffectExecuted = false;

            Action<Error> sideEffect = err => sideEffectExecuted = true;

            var result = sut.TeeError(sideEffect);

            sideEffectExecuted.Should().BeTrue();
            ResultAssert.Failure(result, error);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_not_execute_side_effect_on_success()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            var sideEffectExecuted = false;

            Action<Error> sideEffect = _ => sideEffectExecuted = true;

            var result = sut.TeeError(sideEffect);

            sideEffectExecuted.Should().BeFalse();
            ResultAssert.Success(result, Unit.value);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public void Should_return_same_result_instance()
        {
            Result<Unit, Error> sut = UnitResult.Fail(Errors.Error1);
            Action<Error> sideEffect = _ => { };

            var result = sut.TeeError(sideEffect);

            result.Should().BeSameAs(sut);
        }
    }

    [Trait(Category, TeeErrorAsyncCategory)]
    public class TeeErrorAsyncTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_side_effect_is_null()
        {
            Result<Unit, Error> sut = UnitResult.Fail(Errors.Error1);
            Func<Error, Task> sideEffectAsync = null!;

            await sut.Invoking(res => res.TeeErrorAsync(sideEffectAsync))
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(sideEffectAsync));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_rethrow_side_effect_exception()
        {
            Result<Unit, Error> sut = UnitResult.Fail(Errors.Error1);
            var exception = new InvalidOperationException();
            Func<Error, Task> sideEffectAsync = _ => Task.FromException(exception);

            (await sut.Invoking(res => res.TeeErrorAsync(sideEffectAsync))
                .Should()
                .ThrowAsync<InvalidOperationException>())
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_execute_side_effect_on_failure()
        {
            var error = Errors.Error1;
            Result<Unit, Error> sut = Result.Fail<Unit, Error>(error);
            var sideEffectExecuted = false;

            Func<Error, Task> sideEffectAsync = _ =>
            {
                sideEffectExecuted = true;
                return Task.CompletedTask;
            };

            var result = await sut.TeeErrorAsync(sideEffectAsync);

            sideEffectExecuted.Should().BeTrue();
            ResultAssert.Failure(result, error);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_not_execute_side_effect_on_success()
        {
            Result<Unit, Error> sut = UnitResult.Succeed();
            var sideEffectExecuted = false;

            Func<Error, Task> sideEffectAsync = _ =>
            {
                sideEffectExecuted = true;
                return Task.CompletedTask;
            };

            var result = await sut.TeeErrorAsync(sideEffectAsync);

            sideEffectExecuted.Should().BeFalse();
            ResultAssert.Success(result, Unit.value);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_return_same_result_instance()
        {
            Result<Unit, Error> sut = UnitResult.Fail(Errors.Error1);
            Func<Error, Task> sideEffectAsync = _ => Task.CompletedTask;

            var result = await sut.TeeErrorAsync(sideEffectAsync);

            result.Should().BeSameAs(sut);
        }
    }

    [Trait(Category, ExtensionsTeeErrorCategory)]
    public class ExtensionsTeeErrorTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_side_effect_is_null()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Fail(Errors.Error1).AsTask();
            Action<Error> sideEffect = null!;

            await sut.Invoking(res => res.Tee(sideEffect))
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(sideEffect));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_rethrow_side_effect_exception()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Fail(Errors.Error1).AsTask();
            var exception = new InvalidOperationException();
            Action<Error> sideEffect = _ => throw exception;

            (await sut.Invoking(res => res.Tee(sideEffect))
                .Should()
                .ThrowAsync<InvalidOperationException>())
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_execute_side_effect_on_failure()
        {
            var error = Errors.Error1;
            Task<Result<Unit, Error>> sut = Task.FromResult(Result.Fail<Unit, Error>(error));
            var sideEffectExecuted = false;

            Action<Error> sideEffect = err => sideEffectExecuted = true;

            var result = await sut.Tee(sideEffect);

            sideEffectExecuted.Should().BeTrue();
            ResultAssert.Failure(result, error);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_not_execute_side_effect_on_success()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            var sideEffectExecuted = false;

            Action<Error> sideEffect = _ => sideEffectExecuted = true;

            var result = await sut.Tee(sideEffect);

            sideEffectExecuted.Should().BeFalse();
            ResultAssert.Success(result, Unit.value);
        }
    }

    [Trait(Category, ExtensionsTeeErrorAsyncCategory)]
    public class ExtensionsTeeErrorAsyncTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public async Task Should_throw_exception_when_side_effect_is_null()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Fail(Errors.Error1).AsTask();
            Func<Error, Task> sideEffectAsync = null!;

            await sut.Invoking(res => res.TeeAsync(sideEffectAsync))
                .Should()
                .ThrowAsync<ArgumentNullException>()
                .WithParameterName(nameof(sideEffectAsync));
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_rethrow_side_effect_exception()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Fail(Errors.Error1).AsTask();
            var exception = new InvalidOperationException();
            Func<Error, Task> sideEffectAsync = _ => Task.FromException(exception);

            (await sut.Invoking(res => res.TeeAsync(sideEffectAsync))
                .Should()
                .ThrowAsync<InvalidOperationException>())
                .Which.Should().BeSameAs(exception);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_execute_side_effect_on_failure()
        {
            var error = Errors.Error1;
            Task<Result<Unit, Error>> sut = Task.FromResult(Result.Fail<Unit, Error>(error));
            var sideEffectExecuted = false;

            Func<Error, Task> sideEffectAsync = _ =>
            {
                sideEffectExecuted = true;
                return Task.CompletedTask;
            };

            var result = await sut.TeeAsync(sideEffectAsync);

            sideEffectExecuted.Should().BeTrue();
            ResultAssert.Failure(result, error);
        }

        [Fact]
        [Trait(Area, Invariants)]
        public async Task Should_not_execute_side_effect_on_success()
        {
            Task<Result<Unit, Error>> sut = UnitResult.Succeed<Error>().AsTask();
            var sideEffectExecuted = false;

            Func<Error, Task> sideEffectAsync = _ =>
            {
                sideEffectExecuted = true;
                return Task.CompletedTask;
            };

            var result = await sut.TeeAsync(sideEffectAsync);

            sideEffectExecuted.Should().BeFalse();
            ResultAssert.Success(result, Unit.value);
        }
    }
}
