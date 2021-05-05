using Akka.Actor;
using Akka.Persistence.TestKit;
using Akkatecture.Aggregates;
using AutoFixture.Xunit2;
using CConv.Domain.Models.Common;
using CConv.Domain.Models.UserModel;
using CConv.Domain.Models.UserModel.Commands;
using CConv.Domain.Models.UserModel.Events;
using CConv.Domain.Models.UserModel.Queries;
using CConv.Domain.Models.UserModel.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using Monads;
using Xunit;

namespace CConv.Tests.Domain.Models.UserModel
{
    public sealed class UserAggregateTests : PersistenceTestKit
    {
        [Theory]
        [AutoData]
        public void When_New_CreateUser_Should_Success( UserLogin userLogin, UserPassword password )
        {
            var userManager   = ActorOf<UserAggregateManager>();
            var eventProbe    = CreateTestProbe();
            var createCommand = new CreateUser(userLogin, password);
            Sys.EventStream.Subscribe(eventProbe, typeof(IDomainEvent<UserAggregate, UserId, UserCreated>));
            userManager.Tell(createCommand);
            ExpectMsg<IResult<Nothing, string>>().IsSuccess().Should().BeTrue();
            var domainEvent = eventProbe.ExpectMsg<IDomainEvent<UserAggregate, UserId, UserCreated>>();
            domainEvent.AggregateEvent.Login.Should().Be(userLogin);
            domainEvent.AggregateEvent.Password.Should().Be(password);
        }

        [Theory]
        [AutoData]
        public void When_NotNew_CreateUser_Should_Fail( UserLogin login, UserPassword password1, UserPassword password2 )
        {
            var userManager    = ActorOf<UserAggregateManager>();
            var resultProbe    = CreateTestProbe();
            var createCommand  = new CreateUser(login, password1);
            var createCommand2 = new CreateUser(login, password2);
            userManager.Tell(createCommand);
            userManager.Tell(createCommand2, resultProbe);
            resultProbe.ExpectMsg<IResult<Nothing, string>>()
                       .IsSuccess().Should().BeFalse();
        }

        [Theory]
        [AutoData]
        public void When_New_GetUserById_Should_Fail( UserLogin login )
        {
            var userManager = ActorOf<UserAggregateManager>();
            var userId      = UserId.ForLogin(login);
            var query       = new GetUserById(userId);
            userManager.Tell(query);
            ExpectMsg<IResult<User, string>>().IsSuccess().Should().BeFalse();
        }

        [Theory]
        [AutoData]
        public void When_NotNew_GetUserById_Should_Success( UserLogin login, UserPassword password )
        {
            var userManager   = ActorOf<UserAggregateManager>();
            var resultProbe   = CreateTestProbe();
            var createCommand = new CreateUser(login, password);
            var query         = new GetUserById(UserId.ForLogin(login));
            userManager.Tell(createCommand);
            userManager.Tell(query, resultProbe);
            var user = resultProbe.ExpectMsg<IResult<User, string>>()
                                  .Fold(u => u, f => throw new AssertionFailedException(f));
            user.Login.Should().Be(login);
            user.Password.Should().Be(password);
        }
    }
}