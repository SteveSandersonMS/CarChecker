using System.Security.Claims;

namespace CarChecker.Client.Data
{
    public static class UserExtensions
    {
        public static string FirstName(this ClaimsPrincipal user)
            => user.FindFirst("firstname").Value;

        public static string LastName(this ClaimsPrincipal user)
            => user.FindFirst("lastname").Value;

        public static string Email(this ClaimsPrincipal user)
            => user.Identity.Name;
    }
}
