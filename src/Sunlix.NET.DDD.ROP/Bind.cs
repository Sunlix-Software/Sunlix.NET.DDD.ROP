using Sunlix.NET.DDD.ROP.Extensions;

namespace Sunlix.NET.DDD.ROP
{
    public abstract partial class Result<TSuccess, TFailure>
    {
        public Result<TNewSuccess, TFailure> Bind<TNewSuccess>(Func<TSuccess, Result<TNewSuccess, TFailure>> bindFunction)
        {
            _ = bindFunction ?? throw new ArgumentNullException(nameof(bindFunction));
            return BindInternal(bindFunction);
        }

        public async Task<Result<TNewSuccess, TFailure>> BindAsync<TNewSuccess>(Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> bindFunctionAsync)
        {
            _ = bindFunctionAsync ?? throw new ArgumentNullException(nameof(bindFunctionAsync));
            return await BindInternalAsync(bindFunctionAsync).ConfigureAwait(false);
        }

        protected abstract Result<TNewSuccess, TFailure> BindInternal<TNewSuccess>(Func<TSuccess, Result<TNewSuccess, TFailure>> bindFunction);

        protected abstract Task<Result<TNewSuccess, TFailure>> BindInternalAsync<TNewSuccess>(Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> bindFunctionAsync);


        #region Success & Failure
        private sealed partial class Success : Result<TSuccess, TFailure>
        {
            override protected Result<TNewSuccess, TFailure> BindInternal<TNewSuccess>(Func<TSuccess, Result<TNewSuccess, TFailure>> bindFunction) => bindFunction(Value);

            override protected Task<Result<TNewSuccess, TFailure>> BindInternalAsync<TNewSuccess>(Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> bindFunctionAsync) => bindFunctionAsync(Value);
        }
        private sealed partial class Failure : Result<TSuccess, TFailure>
        {
            override protected Result<TNewSuccess, TFailure> BindInternal<TNewSuccess>(Func<TSuccess, Result<TNewSuccess, TFailure>> bindFunction) => Result.Fail(Error);

            override protected Task<Result<TNewSuccess, TFailure>> BindInternalAsync<TNewSuccess>(Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> bindFunctionAsync)
                => Result.Fail<TNewSuccess, TFailure>(Error).AsTask();
        }
        #endregion
    }

    public static class BindExtensions
    {
        public static async Task<Result<TNewSuccess, TFailure>> Bind<TNewSuccess, TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Func<TSuccess, Result<TNewSuccess, TFailure>> bindFunction)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = bindFunction ?? throw new ArgumentNullException(nameof(bindFunction));

            var result = await taskResult.ConfigureAwait(false);
            return result.Bind(bindFunction);
        }

        public static async Task<Result<TNewSuccess, TFailure>> BindAsync<TNewSuccess, TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> bindFunctionAsync)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = bindFunctionAsync ?? throw new ArgumentNullException(nameof(bindFunctionAsync));

            var result = await taskResult.ConfigureAwait(false);
            return await result.BindAsync(bindFunctionAsync).ConfigureAwait(false);
        }
    }
}
