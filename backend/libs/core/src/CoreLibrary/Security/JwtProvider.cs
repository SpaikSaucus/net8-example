using CoreLibrary.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoreLibrary.Security
{
    public class JwtProvider() : ITokenProvider
    {
        private const string msgErrorEmptySecretKey = "JWT: Secret key 'SymmetricKey' is empty";

        public string GenerateToken(IUserInformation userInformation) 
        {
            var claims = new List<Claim>
            {
                new(ClaimCustomTypes.Guid, userInformation.Guid),
                new(ClaimTypes.NameIdentifier, userInformation.UserName),
                new(ClaimTypes.Name, userInformation.FirstName),
                new(ClaimTypes.Surname, userInformation.LastName),
                new(ClaimTypes.Email, userInformation.Email)
            };

            var secretKey = Environment.GetEnvironmentVariable("JWT.SymmetricKey");
            if (secretKey == null || secretKey == string.Empty) throw new TechnicalException(msgErrorEmptySecretKey);

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = claims.ToDictionary(x => x.Type, x => (object)x.Value),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(createdToken);
        }
    }
}
