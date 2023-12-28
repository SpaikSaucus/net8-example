using Microsoft.AspNetCore.Http;

namespace CoreLibrary.Security
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly Lazy<ClaimsUser> resolver;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            var claims = this.httpContextAccessor.HttpContext.User.Claims;
            this.resolver = new Lazy<ClaimsUser>(() => new ClaimsUser(claims), true);
        }

        public IUserInformation User => this.resolver.Value;
    }
}
