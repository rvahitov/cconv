using Akka.Actor;
using CConv.Domain.Models.Common;

namespace CConv.Domain.Models.UserModel.Commands.Handlers
{
    internal sealed class CreateUserHandler : BaseCommandHandler<UserAggregate, UserId, CreateUser>
    {
        public CreateUserHandler( IActorRef appActor ) : base(appActor)
        {
        }
    }
}