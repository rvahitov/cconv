using Akkatecture.Aggregates;
using Akkatecture.Commands;

namespace CConv.Domain.Models.UserModel
{
    public sealed class UserAggregateManager
        : AggregateManager<UserAggregate, UserId, Command<UserAggregate, UserId>>
    {
    }
}