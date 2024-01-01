using CoreLibrary.Exceptions;
using System.Security.Claims;

namespace CoreLibrary.Security
{
    public class ClaimCustomTypes
    {
        /// <summary>
        /// Check if it does not exist in this list: https://learn.microsoft.com/es-es/dotnet/api/system.security.claims.claimtypes?view=net-8.0 
        /// If it exists, the collision means that the custom type is replaces with http://schemas.xmlsoap.org/ws/2005/05/identity/claims/[text] 
        /// and throws "ErrorClaimNotExist" when you run the constructor for ClaimsUser.
        /// </summary>
        public const string Guid = "guid";
    }

    public class ClaimsUser : IUserInformation
    {
        private const string ErrorClaimNotExist = "The UserClaims is not complete";

        public string Guid { get; }
        public string UserName { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }

        public ClaimsUser(IEnumerable<Claim> claims)
        {
            var enumerable = claims as Claim[] ?? claims.ToArray();

            try
            {
                this.Guid = enumerable.First(c => c.Type == ClaimCustomTypes.Guid).Value;
                this.UserName = enumerable.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                this.FirstName = enumerable.First(c => c.Type == ClaimTypes.Name).Value;
                this.LastName = enumerable.First(c => c.Type == ClaimTypes.Surname).Value;
                this.Email = enumerable.First(c => c.Type == ClaimTypes.Email).Value;
            }
            catch (Exception)
            {
                throw new TechnicalException(ErrorClaimNotExist);
            }
        }
    }
}
