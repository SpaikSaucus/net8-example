using System.Collections.Generic;
using System.Security.Claims;

namespace CoreLibrary.Security
{
    public interface ITokenProvider
    {
        string GenerateToken(IUserInformation userInformation);
    }
}
