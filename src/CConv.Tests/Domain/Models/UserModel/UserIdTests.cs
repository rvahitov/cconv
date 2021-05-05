using AutoFixture.Xunit2;
using CConv.Domain.Models.UserModel;
using CConv.Domain.Models.UserModel.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CConv.Tests.Domain.Models.UserModel
{
    public sealed class UserIdTests
    {
        [Theory]
        [AutoData]
        public void UserId_Should_Equal( UserLogin login )
        {
            UserId id1 = UserId.ForLogin(login);
            UserId id2 = new UserId(id1.Value);
            id2.Should().Be(id1);
        }
    }
}