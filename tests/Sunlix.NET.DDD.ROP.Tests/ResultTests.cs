using Sunlix.NET.DDD.BaseTypes;

using static Sunlix.NET.DDD.ROP.Tests.Traits;

namespace Sunlix.NET.DDD.ROP.Tests
{
    [Trait(Category, ResultCategory)]
    public class ResultTests
    {
        [Trait(Area, Invariants)]
        [Fact]
        public void Succeed_should_throw_when_value_is_null()
        {
            var action = () => Result.Succeed<string, Error>(null!);
            action.Should().Throw<ArgumentNullException>();
        }

        [Trait(Area, Invariants)]
        [Fact]
        public void Fail_should_throw_when_error_is_null()
        {
            var action = () => Result.Fail<string, Error>(null!);
            action.Should().Throw<ArgumentNullException>();
        }

        [Trait(Area, Invariants)]
        [Fact]
        public void Succeed_should_return_success_with_expected_value()
        {
            var result = Result.Succeed<string, Error>("Success");
            ResultAssert.Success(result, "Success");
        }

        [Trait(Area, Invariants)]
        [Fact]
        public void Succeed_with_value_type_should_allow_default_value()
        {
            var result = Result.Succeed<int, Error>(0);
            ResultAssert.Success(result, 0);
        }

        [Trait(Area, Invariants)]
        [Fact]
        public void Fail_should_return_failure_with_expected_error()
        {
            var result = Result.Fail<string, Error>(Errors.Error1);
            ResultAssert.Failure(result, Errors.Error1);
        }

        [Trait(Area, ImplicitConversion)]
        [Fact]
        public void Implicit_conversion_from_GenericSuccess_should_throw_when_value_is_null()
        {
            var genericSuccess = Result.Succeed<string>(null!);
            Action act = () => { Result<string, Error> _ = genericSuccess; };

            act.Should().Throw<ArgumentNullException>();
        }

        [Trait(Area, ImplicitConversion)]
        [Fact]
        public void Implicit_conversion_from_GenericSuccess_should_return_success_with_expected_value()
        {
            Result<string, Error> result = Result.Succeed("Success");
            ResultAssert.Success(result, "Success");
        }

        [Trait(Area, ImplicitConversion)]
        [Fact]
        public void Implicit_conversion_from_GenericFailure_should_throw_when_error_is_null()
        {
            var genericFailure = Result.Fail<Error>(null!);
            Action act = () => { Result<string, Error> _ = genericFailure; };

            act.Should().Throw<ArgumentNullException>();
        }

        [Trait(Area, ImplicitConversion)]
        [Fact]
        public void Implicit_conversion_from_GenericFailure_should_return_failure_with_expected_error()
        {
            Result<string, Error> result = Result.Fail(Errors.Error1);
            ResultAssert.Failure(result, Errors.Error1);
        }

        [Trait(Area, ImplicitConversion)]
        [Fact]
        public void Implicit_conversion_from_error_should_throw_when_error_is_null()
        {
            Error error = null!;
            Action act = () => { Result<string, Error> _ = error; };

            act.Should().Throw<ArgumentNullException>();
        }

        [Trait(Area, ImplicitConversion)]
        [Fact]
        public void Implicit_conversion_from_error_should_return_failure_with_expected_error()
        {
            Result<string, Error> result = Errors.Error1;
            ResultAssert.Failure(result, Errors.Error1);
        }

        [Trait(Area, ImplicitConversion)]
        [Fact]
        public void Implicit_conversion_from_default_GenericSuccess_should_throw_when_converted()
        {
            Result.GenericSuccess<string> genericSuccess = default;
            Action act = () => { Result<string, Error> _ = genericSuccess; };
            
            act.Should().Throw<ArgumentNullException>();
        }

        [Trait(Area, ImplicitConversion)]
        [Fact]
        public void Implicit_conversion_from_default_GenericFailure_should_throw_when_converted()
        {
            Result.GenericFailure<Error> genericFailure = default;
            Action act = () => { Result<string, Error> _ = genericFailure; };
            
            act.Should().Throw<ArgumentNullException>();
        }

        [Trait(Area, Traits.UnitResult)]
        [Fact]
        public void UnitResult_succeed_should_return_success()
        {
            var result = UnitResult.Succeed<Error>();
            ResultAssert.Success(result, Unit.value);
        }

        [Trait(Area, Traits.UnitResult)]
        [Fact]
        public void UnitResult_fail_should_return_failure()
        {
            var result = UnitResult.Fail(Errors.Error1);
            ResultAssert.Failure(result, Errors.Error1);
        }
    }
}
