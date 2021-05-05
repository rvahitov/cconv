using Akkatecture.Entities;
using CConv.Domain.Models.UserModel.ValueObjects;

namespace CConv.Domain.Models.UserModel
{
    public sealed class User : Entity<UserId>
    {
        public User( UserId id, UserLogin login, UserPassword password ) : base(id)
        {
            Login    = login;
            Password = password;
        }

        public UserLogin    Login    { get; }
        public UserPassword Password { get; }
    }
}