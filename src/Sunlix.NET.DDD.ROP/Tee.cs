namespace Sunlix.NET.DDD.ROP
{
    public abstract partial class Result<TSuccess, TFailure>
    {
        public Result<TSuccess, TFailure> Tee(Action<TSuccess> sideEffect)
        {
            _ = sideEffect ?? throw new ArgumentNullException(nameof(sideEffect));
            return TeeInternal(sideEffect);
        }

        public async Task<Result<TSuccess, TFailure>> TeeAsync(Func<TSuccess, Task> sideEffectAsync)
        {
            _ = sideEffectAsync ?? throw new ArgumentNullException(nameof(sideEffectAsync));
            return await TeeInternalAsync(sideEffectAsync).ConfigureAwait(false);
        }


        protected abstract Result<TSuccess, TFailure> TeeInternal(Action<TSuccess> sideEffect);
        protected abstract Task<Result<TSuccess, TFailure>> TeeInternalAsync(Func<TSuccess, Task> sideEffectAsync);


        private sealed partial class Success
        {
            protected override Result<TSuccess, TFailure> TeeInternal(Action<TSuccess> sideEffect)
            {
                sideEffect(Value);
                return this;
            }

            protected override async Task<Result<TSuccess, TFailure>> TeeInternalAsync(Func<TSuccess, Task> sideEffectAsync)
            {
                await sideEffectAsync(Value).ConfigureAwait(false);
                return this;
            }
        }

        private sealed partial class Failure
        {
            protected override Result<TSuccess, TFailure> TeeInternal(Action<TSuccess> sideEffect) => this;

            protected override Task<Result<TSuccess, TFailure>> TeeInternalAsync(Func<TSuccess, Task> sideEffectAsync)
                => Task.FromResult<Result<TSuccess, TFailure>>(this);
        }
    }

    public static class TeeExtensions
    {
        public static async Task<Result<TSuccess, TFailure>> Tee<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Action<TSuccess> sideEffect)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = sideEffect ?? throw new ArgumentNullException(nameof(sideEffect));
            var result = await taskResult.ConfigureAwait(false);
            return result.Tee(sideEffect);
        }

        public static async Task<Result<TSuccess, TFailure>> TeeAsync<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Func<TSuccess, Task> sideEffectAsync)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = sideEffectAsync ?? throw new ArgumentNullException(nameof(sideEffectAsync));
            var result = await taskResult.ConfigureAwait(false);
            return await result.TeeAsync(sideEffectAsync).ConfigureAwait(false);
        }
    }
}
