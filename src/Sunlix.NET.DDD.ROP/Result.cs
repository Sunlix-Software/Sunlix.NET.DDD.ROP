using Sunlix.NET.DDD.BaseTypes;

namespace Sunlix.NET.DDD.ROP
{
    public abstract partial class Result<TSuccess, TFailure>
    {
        private Result() { }

        public bool IsSuccess => this is Success;
        public bool IsFailure => this is Failure;

        public abstract TSuccess Value { get; }
        public abstract TFailure Error { get; }

        public static implicit operator Result<TSuccess, TFailure>(Result.GenericSuccess<TSuccess> success) => new Success(success.Value);
        public static implicit operator Result<TSuccess, TFailure>(Result.GenericFailure<TFailure> failure) => new Failure(failure.Error);
        public static implicit operator Result<TSuccess, TFailure>(TFailure error) => new Failure(error);


        #region Success & Failure
        private sealed partial class Success : Result<TSuccess, TFailure>
        {
            public override TSuccess Value { get; }
            public override TFailure Error => throw new InvalidOperationException("Accessing Error on Success.");
            public Success(TSuccess value)
            {
                ArgumentNullException.ThrowIfNull(value);
                Value = value;
            }
        }
        private sealed partial class Failure : Result<TSuccess, TFailure>
        {
            public override TSuccess Value => throw new InvalidOperationException("Accessing Value on Failure.");
            public override TFailure Error { get; }
            public Failure(TFailure error)
            {
                ArgumentNullException.ThrowIfNull(error);
                Error = error;
            }
        }
        #endregion
    }

    public static class Result
    {
        public static Result<TSuccess, TFailure> Succeed<TSuccess, TFailure>(TSuccess value) => Succeed(value);

        public static GenericSuccess<TSuccess> Succeed<TSuccess>(TSuccess value) => new(value);

        public static Result<TSuccess, TFailure> Fail<TSuccess, TFailure>(TFailure error) => Fail(error);

        public static GenericFailure<TFailure> Fail<TFailure>(TFailure error) => new(error);


        #region GenericSuccess & GenericFailure
        public readonly struct GenericSuccess<T>
        {
            internal T Value { get; }
            internal GenericSuccess(T value) => Value = value;
        }

        public readonly struct GenericFailure<T>
        {
            internal T Error { get; }
            internal GenericFailure(T error) => Error = error;
        }
        #endregion
    }
    public static class UnitResult
    {
        public static Result<Unit, TFailure> Succeed<TFailure>() => Result.Succeed(Unit.value);
        public static Result<Unit, TFailure> Fail<TFailure>(TFailure error) => Result.Fail(error);
        public static Result.GenericSuccess<Unit> Succeed() => Result.Succeed(Unit.value);
    }
}
