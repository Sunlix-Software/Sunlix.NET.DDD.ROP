namespace Sunlix.NET.DDD.ROP
{
    public abstract partial class Result<TSuccess, TFailure>
    {
        /// <include file="XmlDocs/TeeError.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.TeeError(System.Action{`1})']/*" />
        public Result<TSuccess, TFailure> TeeError(Action<TFailure> sideEffect)
        {
            _ = sideEffect ?? throw new ArgumentNullException(nameof(sideEffect));
            return TeeErrorInternal(sideEffect);
        }

        /// <include file="XmlDocs/TeeError.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.TeeErrorAsync(System.Func{`1,System.Threading.Tasks.Task})']/*" />
        public async Task<Result<TSuccess, TFailure>> TeeErrorAsync(Func<TFailure, Task> sideEffectAsync)
        {
            _ = sideEffectAsync ?? throw new ArgumentNullException(nameof(sideEffectAsync));
            return await TeeErrorInternalAsync(sideEffectAsync).ConfigureAwait(false);
        }


        /// <include file="XmlDocs/TeeError.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.TeeErrorInternal(System.Action{`1})']/*" />
        protected abstract Result<TSuccess, TFailure> TeeErrorInternal(Action<TFailure> sideEffect);
        /// <include file="XmlDocs/TeeError.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.TeeErrorInternalAsync(System.Func{`1,System.Threading.Tasks.Task})']/*" />
        protected abstract Task<Result<TSuccess, TFailure>> TeeErrorInternalAsync(Func<TFailure, Task> sideEffectAsync);


        private sealed partial class Success
        {
            protected override Result<TSuccess, TFailure> TeeErrorInternal(Action<TFailure> sideEffect) => this;

            protected override Task<Result<TSuccess, TFailure>> TeeErrorInternalAsync(Func<TFailure, Task> sideEffectAsync)
                => Task.FromResult<Result<TSuccess, TFailure>>(this);
        }

        private sealed partial class Failure
        {
            

            protected override Result<TSuccess, TFailure> TeeErrorInternal(Action<TFailure> sideEffect)
            {
                sideEffect(Error);
                return this;
            }

            protected override async Task<Result<TSuccess, TFailure>> TeeErrorInternalAsync(Func<TFailure, Task> sideEffectAsync)
            {
                await sideEffectAsync(Error).ConfigureAwait(false);
                return this;
            }
        }
    }

    /// <include file="XmlDocs/TeeError.xml" path="doc/members/member[@name='T:Sunlix.NET.DDD.ROP.TeeErrorExtensions']/*" />
    public static class TeeErrorExtensions
    {
        /// <include file="XmlDocs/TeeError.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.TeeErrorExtensions.Tee``2(System.Threading.Tasks.Task{Sunlix.NET.DDD.ROP.Result{``0,``1}},System.Action{``1})']/*" />
        public static async Task<Result<TSuccess, TFailure>> Tee<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Action<TFailure> sideEffect)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = sideEffect ?? throw new ArgumentNullException(nameof(sideEffect));
            var result = await taskResult.ConfigureAwait(false);
            return result.TeeError(sideEffect);
        }

        /// <include file="XmlDocs/TeeError.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.TeeErrorExtensions.TeeAsync``2(System.Threading.Tasks.Task{Sunlix.NET.DDD.ROP.Result{``0,``1}},System.Func{``1,System.Threading.Tasks.Task})']/*" />
        public static async Task<Result<TSuccess, TFailure>> TeeAsync<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Func<TFailure, Task> sideEffectAsync)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = sideEffectAsync ?? throw new ArgumentNullException(nameof(sideEffectAsync));
            var result = await taskResult.ConfigureAwait(false);
            return await result.TeeErrorAsync(sideEffectAsync).ConfigureAwait(false);
        }
    }
}
