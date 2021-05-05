using Akkatecture.Aggregates;
using CConv.Domain.Models.Common;
using CConv.Domain.Models.UserModel.Commands;
using CConv.Domain.Models.UserModel.Events;

namespace CConv.Domain.Models.UserModel
{
    public sealed class UserAggregate
        : AggregateRoot<UserAggregate, UserId, UserState>
    {
        public UserAggregate( UserId id ) : base(id)
        {
            Command<CreateUser>(OnCreate);
        }

        private void OnCreate( CreateUser command )
        {
            if ( IsNew == false )
            {
                Sender.Tell(ExecutionResult.Failure($"User {Id} already exists"),Self);
                return;
            }

            Reply(ExecutionResult.Success());
            var e = new UserCreated(command.Login, command.Password);
            Emit(e);
        }
    }
}