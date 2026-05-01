using Sunlix.NET.DDD.ROP.Extensions;

namespace Sunlix.NET.DDD.ROP
{
    public abstract partial class Result<TSuccess, TFailure>
    {
        /// <include file="XmlDocs/Bind.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.Bind``1(System.Func{`0,Sunlix.NET.DDD.ROP.Result{``0,`1}})']/*" />
        public Result<TNewSuccess, TFailure> Bind<TNewSuccess>(Func<TSuccess, Result<TNewSuccess, TFailure>> bindFunction)
        {
            _ = bindFunction ?? throw new ArgumentNullException(nameof(bindFunction));
            return BindInternal(bindFunction);
        }

        /// <include file="XmlDocs/Bind.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.BindAsync``1(System.Func{`0,System.Threading.Tasks.Task{Sunlix.NET.DDD.ROP.Result{``0,`1}}})']/*" />
        public async Task<Result<TNewSuccess, TFailure>> BindAsync<TNewSuccess>(Func<TSuccess, Task<Result<TNewSuccess, TFailure>>> bindFunctionAsync)
        {
            _ = bindFunctionAsync ?? throw new ArgumentNullException(nameof(bindFunctionAsync));
            return await BindInternalAsync(bindFunctionAsync).ConfigureAwait(false);
        }

        /// <include file="XmlDocs/Bind.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.BindInternal``1(System.Func{`0,Sunlix.NET.DDD.ROP.Result{``0,`1}})']/*" />
        protected abstract Result<TNewSuccess, TFailure> BindInternal<TNewSuccess>(Func<TSuccess, Result<TNewSuccess, TFailure>> bindFunction);

        /// <include file="XmlDocs/Bind.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.BindInternalAsync``1(System.Func{`0,System.Threading.Tasks.Task{Sunlix.NET.DDD.ROP.Result{``0,`1}}})']/*" />
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

    /// <include file="XmlDocs/Bind.xml" path="doc/members/member[@name='T:Sunlix.NET.DDD.ROP.BindExtensions']/*" />
    public static class BindExtensions
    {
        /// <include file="XmlDocs/Bind.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.BindExtensions.Bind``3(System.Threading.Tasks.Task{Sunlix.NET.DDD.ROP.Result{``1,``2}},System.Func{``1,Sunlix.NET.DDD.ROP.Result{``0,``2}})']/*" />
        public static async Task<Result<TNewSuccess, TFailure>> Bind<TNewSuccess, TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Func<TSuccess, Result<TNewSuccess, TFailure>> bindFunction)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = bindFunction ?? throw new ArgumentNullException(nameof(bindFunction));

            var result = await taskResult.ConfigureAwait(false);
            return result.Bind(bindFunction);
        }

        /// <include file="XmlDocs/Bind.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.BindExtensions.BindAsync``3(System.Threading.Tasks.Task{Sunlix.NET.DDD.ROP.Result{``1,``2}},System.Func{``1,System.Threading.Tasks.Task{Sunlix.NET.DDD.ROP.Result{``0,``2}}})']/*" />
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
