using Akkatecture.Commands;
using MediatR;
using Monads;

namespace CConv.Domain.Models.UserModel.Queries
{
    public sealed class GetUserById 
        : Command<UserAggregate,UserId>, IRequest<IResult<User,string>>
    {
        public GetUserById( UserId aggregateId ) : base(aggregateId)
        {
        }
    }
}