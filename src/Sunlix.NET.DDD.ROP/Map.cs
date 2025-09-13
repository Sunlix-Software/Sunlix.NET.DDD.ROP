using Sunlix.NET.DDD.ROP.Extensions;

namespace Sunlix.NET.DDD.ROP
{
    public abstract partial class Result<TSuccess, TFailure>
    {
        public Result<TNewSuccess, TFailure> Map<TNewSuccess>(Func<TSuccess, TNewSuccess> mapFunction)
        {
            _ = mapFunction ?? throw new ArgumentNullException(nameof(mapFunction));
            return MapInternal(mapFunction);
        }

        public async Task<Result<TNewSuccess, TFailure>> MapAsync<TNewSuccess>(Func<TSuccess, Task<TNewSuccess>> mapFunctionAsync)
        {
            _ = mapFunctionAsync ?? throw new ArgumentNullException(nameof(mapFunctionAsync));
            return await MapInternalAsync(mapFunctionAsync).ConfigureAwait(false);
        }

        protected abstract Result<TNewSuccess, TFailure> MapInternal<TNewSuccess>(Func<TSuccess, TNewSuccess> mapFunction);
        protected abstract Task<Result<TNewSuccess, TFailure>> MapInternalAsync<TNewSuccess>(Func<TSuccess, Task<TNewSuccess>> mapFunctionAsync);

        #region Success & Failure
        private sealed partial class Success : Result<TSuccess, TFailure>
        {
            override protected Result<TNewSuccess, TFailure> MapInternal<TNewSuccess>(Func<TSuccess, TNewSuccess> mapFunction) => Result.Succeed(mapFunction(Value));
            override protected async Task<Result<TNewSuccess, TFailure>> MapInternalAsync<TNewSuccess>(Func<TSuccess, Task<TNewSuccess>> mapFunctionAsync)
            {
                var mapResult = await mapFunctionAsync(Value).ConfigureAwait(false);
                return Result.Succeed<TNewSuccess, TFailure>(mapResult);
            }
        }
        private sealed partial class Failure : Result<TSuccess, TFailure>
        {
            override protected Result<TNewSuccess, TFailure> MapInternal<TNewSuccess>(Func<TSuccess, TNewSuccess> mapFunction) => Result.Fail(Error);

            override protected Task<Result<TNewSuccess, TFailure>> MapInternalAsync<TNewSuccess>(Func<TSuccess, Task<TNewSuccess>> mapFunctionAsync)
                => Result.Fail<TNewSuccess, TFailure>(Error).AsTask();
        }
        #endregion
    }

    public static class MapExtensions
    {
        public static async Task<Result<TNewSuccess, TFailure>> Map<TNewSuccess, TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Func<TSuccess, TNewSuccess> mapFunction)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = mapFunction ?? throw new ArgumentNullException(nameof(mapFunction));

            var result = await taskResult.ConfigureAwait(false);
            return result.Map(mapFunction);
        }

        public static async Task<Result<TNewSuccess, TFailure>> MapAsync<TNewSuccess, TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Func<TSuccess, Task<TNewSuccess>> mapFunctionAsync)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = mapFunctionAsync ?? throw new ArgumentNullException(nameof(mapFunctionAsync));

            var result = await taskResult.ConfigureAwait(false);
            return await result.MapAsync(mapFunctionAsync).ConfigureAwait(false);
        }
    }
}
