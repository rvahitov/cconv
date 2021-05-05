using Akkatecture.Aggregates;
using CConv.Domain.Models.Common;
using CConv.Domain.Models.UserModel.Commands;
using CConv.Domain.Models.UserModel.Events;
using CConv.Domain.Models.UserModel.Queries;
using Monads;

namespace CConv.Domain.Models.UserModel
{
    public sealed class UserAggregate
        : AggregateRoot<UserAggregate, UserId, UserState>
    {
        public UserAggregate( UserId id ) : base(id)
        {
            Command<CreateUser>(OnCreate);
            Command<GetUserById>(OnGetById);
        }

        private void OnCreate( CreateUser command )
        {
            if ( IsNew == false )
            {
                Sender.Tell(ExecutionResult.Failure($"User {Id} already exists"), Self);
                return;
            }

            Reply(ExecutionResult.Success());
            var e = new UserCreated(command.Login, command.Password);
            Emit(e);
        }

        private void OnGetById( GetUserById obj )
        {
            var x = from _ in IsNew.Validate(isNew => isNew == false, $"User {Id} is not exist")
                    let user = new User(Id, State.Login!, State.Password!)
                    select user;
            x.Fold
            (
                user => Sender.Tell(new SuccessResult<User, string>(user), Self),
                f => Sender.Tell(new FailureResult<User, string>(f), Self)
            );
        }
    }
}