using Akka.Actor;
using CConv.Domain.Models.Common;

namespace CConv.Domain.Models.UserModel.Queries.Handlers
{
    internal sealed class GetUserByIdHandler : BaseQueryHandler<GetUserById, User>
    {
        public GetUserByIdHandler( IActorRef appActor ) : base(appActor)
        {
        }
    }
}