using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace Monads.Tests
{
    public sealed class ResultTests
    {
        [Fact]
        public void SuccessResult_IsSuccess_Should_BeTrue()
        {
            //arrange
            IResult result = new SuccessResult<int, string>(10);
            //assert
            result.IsSuccess().Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void SuccessResult_Fold_Should_Double_Value( int value )
        {
            //arrange
            var result = new SuccessResult<int, string>(value);
            //act
            var newValue = result.Fold(v => v * 2, _ => value);
            //assert
            newValue.Should().Be(value * 2);
        }

        [Fact]
        public void FailureResult_IsSuccess_Should_BeFalse()
        {
            //arrange
            IResult result = new FailureResult<int, string>("Error");
            //assert
            result.IsSuccess().Should().BeFalse();
        }

        [Theory]
        [AutoData]
        public void FailureResult_Fold_Should_Return_OriginValue( int value )
        {
            //arrange
            var result = new FailureResult<int, string>("Error");
            //act
            var newValue = result.Fold(v => v * 2, _ => value);
            //assert
            newValue.Should().Be(value);
        }

        [Theory]
        [AutoData]
        public void Validate_Should_Return_SuccessResult( int value )
        {
            //arrange & act
            var result =
                value.Validate(i => i == value, "Value is not equals value");
            //assert
            result.Should().Be(new SuccessResult<int, string>(value));
        }

        [Theory]
        [AutoData]
        public void Validate_Should_Return_FailureResult( int value )
        {
            //arrange & act
            var result =
                value.Validate(i => i != value, "Value and value are same");
            //assert
            result.Should().Be(new FailureResult<int, string>("Value and value are same"));
        }

        [Theory]
        [AutoData]
        public void SuccessResult_Select_Should_Return_SuccessResult( Guid successValue )
        {
            //arrange
            var result = new SuccessResult<Guid, string>(successValue);
            //act
            var newResult = result.Select(g => g.ToString());
            //assert
            newResult.Should().Be(new SuccessResult<string, string>(successValue.ToString()));
        }

        [Theory]
        [AutoData]
        public void FailureResult_Select_Should_Return_FailureResult( string failure )
        {
            //arrange
            var result = new FailureResult<string, string>(failure);
            //act
            var newResult = result.Select(int.Parse);
            //assert
            newResult.Should().Be(new FailureResult<int, string>(failure));
        }

        [Theory]
        [AutoData]
        public void SuccessResult_SelectMany_Should_Return_SuccessResult( decimal successValue, string error )
        {
            //arrange
            var result = new SuccessResult<decimal, string>(successValue);
            //act
            var newResult = result.SelectMany(v => v.Validate(d => d == successValue, error));
            //assert
            newResult.Should().Be(result);
        }

        [Theory]
        [AutoData]
        public void FailureResult_SelectMany_Should_Return_FailureResult( string error )
        {
            //arrange
            var result = new FailureResult<int, string>(error);
            //act
            var newResult = result.SelectMany(i => i.Validate(v => v != 0, "Int is zero"));
            //assert
            newResult.Should().Be(result);
        }

        [Theory]
        [AutoData]
        public void SuccessResult_SelectMany_Should_Return_FailureResult( int successValue, string error )
        {
            //arrange
            var result = new SuccessResult<int, string>(successValue);
            //act
            var newResult = result.Select(i => i / successValue).SelectMany(i => i.Validate(v => v != 1, error));
            //assert
            newResult.Should().Be(new FailureResult<int, string>(error));
        }

        [Theory]
        [AutoData]
        public void When_SuccessResult_And_SuccessResult_SelectMany_Should_Return_SuccessResult( int s1, double s2 )
        {
            //arrange
            var res1 = new SuccessResult<int, Exception>(s1);
            var res2 = new SuccessResult<double, Exception>(s2);
            //act
            var newResult = from i in res1
                            from d in res2
                            let x = i * d
                            select x;
            //assert
            newResult.Should().Be(new SuccessResult<double, Exception>(s1 * s2));
        }

        [Theory]
        [AutoData]
        public void When_SuccessResult_And_FailureResult_SelectMany_Should_Return_SuccessResult( int s1, string f2 )
        {
            //arrange
            var res1 = new SuccessResult<int, string>(s1);
            var res2 = new FailureResult<double, string>(f2);
            //act
            var newResult = from i in res1
                            from d in res2
                            let x = i * d
                            select x;
            //assert
            newResult.Should().Be(new FailureResult<double, string>(f2));
        }

        [Theory]
        [AutoData]
        public void When_FailureResult_And_FailureResult_SelectMany_Should_Return_SuccessResult( string f1, string f2 )
        {
            //arrange
            var res1 = new FailureResult<int, string>(f1);
            var res2 = new FailureResult<double, string>(f2);
            //act
            var newResult = from i in res1
                            from d in res2
                            let x = i * d
                            select x;
            //assert
            newResult.Should().Be(new FailureResult<double, string>(f1));
        }

        [Theory]
        [AutoData]
        public void When_SuccessResult_Fold_ShouldCall_OnSuccess( int successValue )
        {
            //arrange
            var checkValue = 0;
            var result     = new SuccessResult<int, string>(successValue);

            void OnSuccess( int v ) => checkValue = v * 2;

            void OnFailure( string _ ) => checkValue = successValue;
            //act
            result.Fold(OnSuccess, OnFailure);
            //assert
            checkValue.Should().Be(successValue * 2);
        }

        [Theory]
        [AutoData]
        public void When_FailureResult_Fold_ShouldCall_OnFailure(string successValue, string error )
        {
            //arrange
            var checkValue = "";

            var result = new FailureResult<string, string>(error);

            void OnSuccess( string _ )       => checkValue = successValue;
            void OnFailure( string failure ) => checkValue = failure;
            //act
            result.Fold(OnSuccess, OnFailure);
            //assert
            checkValue.Should().Be(error);
        }
    }
}