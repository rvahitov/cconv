using Akkatecture.Commands;
using CConv.Domain.Models.UserModel.ValueObjects;

namespace CConv.Domain.Models.UserModel.Commands
{
    public sealed class CreateUser : Command<UserAggregate, UserId>
    {
        public CreateUser( UserLogin login, UserPassword password ) : base(UserId.ForLogin(login))
        {
            Login         = login;
            Password = password;
        }

        public UserLogin    Login    { get; }
        public UserPassword Password { get; }
    }
}