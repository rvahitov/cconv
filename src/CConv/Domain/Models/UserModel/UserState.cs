using Akkatecture.Aggregates;
using CConv.Domain.Models.UserModel.Events;
using CConv.Domain.Models.UserModel.ValueObjects;

namespace CConv.Domain.Models.UserModel
{
    public sealed class UserState
        : AggregateState<UserAggregate, UserId>,
          IApply<UserCreated>
    {
        public UserLogin?    Login    { get; private set; }
        public UserPassword? Password { get; private set; }

        public void Apply( UserCreated aggregateEvent )
        {
            Login    = aggregateEvent.Login;
            Password = aggregateEvent.Password;
        }
    }
}