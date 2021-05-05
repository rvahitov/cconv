using Akkatecture.Commands;
using CConv.Domain.Models.Common;
using CConv.Domain.Models.UserModel.ValueObjects;
using MediatR;
using Monads;

namespace CConv.Domain.Models.UserModel.Commands
{
    public sealed class CreateUser
        : Command<UserAggregate, UserId>, IRequest<IResult<Nothing, string>>
    {
        public CreateUser( UserLogin login, UserPassword password ) : base(UserId.ForLogin(login))
        {
            Login    = login;
            Password = password;
        }

        public UserLogin    Login    { get; }
        public UserPassword Password { get; }
    }
}