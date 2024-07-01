
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user?.Claims
                       .SingleOrDefault(x => x.Type == ClaimTypes.GivenName)?
                       .Value ?? "Username not found";
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user?.Claims
                       .SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?
                       .Value ?? "UserId not found";
        }

        public static string GetRole(this ClaimsPrincipal user)
        {
            return user?.Claims
                       .SingleOrDefault(x => x.Type == ClaimTypes.Role)?
                       .Value ?? "Role not found";
        }
    }
}