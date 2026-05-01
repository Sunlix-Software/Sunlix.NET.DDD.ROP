using Sunlix.NET.DDD.BaseTypes;

namespace Sunlix.NET.DDD.ROP
{
    /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='T:Sunlix.NET.DDD.ROP.Result`2']/*" />
    public abstract partial class Result<TSuccess, TFailure>
    {
        private Result() { }

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='P:Sunlix.NET.DDD.ROP.Result`2.IsSuccess']/*" />
        public bool IsSuccess => this is Success;

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='P:Sunlix.NET.DDD.ROP.Result`2.IsFailure']/*" />
        public bool IsFailure => this is Failure;

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='P:Sunlix.NET.DDD.ROP.Result`2.Value']/*" />
        public abstract TSuccess Value { get; }

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='P:Sunlix.NET.DDD.ROP.Result`2.Error']/*" />
        public abstract TFailure Error { get; }

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.op_Implicit(Sunlix.NET.DDD.ROP.Result.GenericSuccess{`0})']/*" />
        public static implicit operator Result<TSuccess, TFailure>(Result.GenericSuccess<TSuccess> success) => new Success(success.Value);

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.op_Implicit(Sunlix.NET.DDD.ROP.Result.GenericFailure{`1})']/*" />
        public static implicit operator Result<TSuccess, TFailure>(Result.GenericFailure<TFailure> failure) => new Failure(failure.Error);

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result`2.op_Implicit(`1)']/*" />
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

    /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='T:Sunlix.NET.DDD.ROP.Result']/*" />
    public static class Result
    {
        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result.Succeed``2(``0)']/*" />
        public static Result<TSuccess, TFailure> Succeed<TSuccess, TFailure>(TSuccess value) => Succeed(value);

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result.Succeed``1(``0)']/*" />
        public static GenericSuccess<TSuccess> Succeed<TSuccess>(TSuccess value) => new(value);

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result.Fail``2(``1)']/*" />
        public static Result<TSuccess, TFailure> Fail<TSuccess, TFailure>(TFailure error) => Fail(error);

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.Result.Fail``1(``0)']/*" />
        public static GenericFailure<TFailure> Fail<TFailure>(TFailure error) => new(error);


        #region GenericSuccess & GenericFailure

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='T:Sunlix.NET.DDD.ROP.Result.GenericSuccess`1']/*" />
        public readonly struct GenericSuccess<T>
        {
            internal T Value { get; }
            internal GenericSuccess(T value) => Value = value;
        }

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='T:Sunlix.NET.DDD.ROP.Result.GenericFailure`1']/*" />
        public readonly struct GenericFailure<T>
        {
            internal T Error { get; }
            internal GenericFailure(T error) => Error = error;
        }
        #endregion
    }

    /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='T:Sunlix.NET.DDD.ROP.UnitResult']/*" />
    public static class UnitResult
    {
        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.UnitResult.Succeed``1']/*" />
            public static Result<Unit, TFailure> Succeed<TFailure>() => Result.Succeed(Unit.value);

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.UnitResult.Fail``1(``0)']/*" />
        public static Result<Unit, TFailure> Fail<TFailure>(TFailure error) => Result.Fail(error);

        /// <include file="XmlDocs/Result.xml" path="doc/members/member[@name='M:Sunlix.NET.DDD.ROP.UnitResult.Succeed']/*" />
        public static Result.GenericSuccess<Unit> Succeed() => Result.Succeed(Unit.value);
    }
}
