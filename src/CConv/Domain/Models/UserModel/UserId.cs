using System;
using Akkatecture.Core;
using CConv.Domain.Models.UserModel.ValueObjects;

namespace CConv.Domain.Models.UserModel
{
    public sealed class UserId : Identity<UserId>
    {
        private static readonly Guid LoginNamespace = new("CEB2ED1C-D86E-4837-B1CF-5702DF594EF2");
        public UserId( string value ) : base(value)
        {
        }

        public static UserId ForLogin( UserLogin login ) => NewDeterministic(LoginNamespace, login.Value);
    }
}