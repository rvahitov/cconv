using Akkatecture.Commands;

namespace CConv.Domain.Models.UserModel.Queries
{
    public sealed class GetUserById : Command<UserAggregate,UserId>
    {
        public GetUserById( UserId aggregateId ) : base(aggregateId)
        {
        }
    }
}