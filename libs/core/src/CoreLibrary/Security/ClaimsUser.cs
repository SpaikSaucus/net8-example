using System.Security.Claims;

namespace CoreLibrary.Security
{
    public class ClaimsUserTypes
    {
        public const string Guid = "guid";
        public const string UserName = "user_name";
        public const string FirstName = "first_name";
        public const string LastName = "last_name";
        public const string Email = "email";
    }

    public class ClaimsUser : IUserInformation
    {
        public string Guid { get; }
        public string UserName { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }

        public ClaimsUser(IEnumerable<Claim> claims)
        {
            var enumerable = claims as Claim[] ?? claims.ToArray();
            this.Guid = enumerable.First(c => c.Type == ClaimsUserTypes.Guid).Value;
            this.UserName = enumerable.First(c => c.Type == ClaimsUserTypes.UserName).Value;
            this.FirstName = enumerable.First(c => c.Type == ClaimsUserTypes.FirstName).Value;
            this.LastName = enumerable.First(c => c.Type == ClaimsUserTypes.LastName).Value;
            this.Email = enumerable.First(c => c.Type == ClaimsUserTypes.Email).Value;
        }
    }
}
