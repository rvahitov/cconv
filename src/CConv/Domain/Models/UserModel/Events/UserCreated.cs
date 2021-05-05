using Akkatecture.Aggregates;
using CConv.Domain.Models.UserModel.ValueObjects;

namespace CConv.Domain.Models.UserModel.Events
{
    public sealed record UserCreated( UserLogin Login, UserPassword Password )
        : IAggregateEvent<UserAggregate, UserId>;
}