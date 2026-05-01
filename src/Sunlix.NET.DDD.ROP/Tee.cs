namespace Sunlix.NET.DDD.ROP
{
    public abstract partial class Result<TSuccess, TFailure>
    {
        /// <include file="XmlDocs/Tee.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.Tee(System.Action{`0})']/*" />
        public Result<TSuccess, TFailure> Tee(Action<TSuccess> sideEffect)
        {
            _ = sideEffect ?? throw new ArgumentNullException(nameof(sideEffect));
            return TeeInternal(sideEffect);
        }

        /// <include file="XmlDocs/Tee.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.TeeAsync(System.Func{`0,System.Threading.Tasks.Task})']/*" />
        public async Task<Result<TSuccess, TFailure>> TeeAsync(Func<TSuccess, Task> sideEffectAsync)
        {
            _ = sideEffectAsync ?? throw new ArgumentNullException(nameof(sideEffectAsync));
            return await TeeInternalAsync(sideEffectAsync).ConfigureAwait(false);
        }


        /// <include file="XmlDocs/Tee.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.TeeInternal(System.Action{`0})']/*" />
        protected abstract Result<TSuccess, TFailure> TeeInternal(Action<TSuccess> sideEffect);
        /// <include file="XmlDocs/Tee.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.TeeInternalAsync(System.Func{`0,System.Threading.Tasks.Task})']/*" />
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

    /// <include file="XmlDocs/Tee.xml" path="doc/members/member[@name='T:Sunlix.NET.DDD.ROP.TeeExtensions']/*" />
    public static class TeeExtensions
    {
        /// <include file="XmlDocs/Tee.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.TeeExtensions.Tee``2(System.Threading.Tasks.Task{Sunlix.NET.DDD.ROP.Result{``0,``1}},System.Action{``0})']/*" />
        public static async Task<Result<TSuccess, TFailure>> Tee<TSuccess, TFailure>(
            this Task<Result<TSuccess, TFailure>> taskResult,
            Action<TSuccess> sideEffect)
        {
            _ = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
            _ = sideEffect ?? throw new ArgumentNullException(nameof(sideEffect));
            var result = await taskResult.ConfigureAwait(false);
            return result.Tee(sideEffect);
        }

        /// <include file="XmlDocs/Tee.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.TeeExtensions.TeeAsync``2(System.Threading.Tasks.Task{Sunlix.NET.DDD.ROP.Result{``0,``1}},System.Func{``0,System.Threading.Tasks.Task})']/*" />
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
