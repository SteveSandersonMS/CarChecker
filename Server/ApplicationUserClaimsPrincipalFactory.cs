using Microsoft.AspNetCore.Identity;
using CarChecker.Server.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CarChecker.Server
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public ApplicationUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("firstname", user.FirstName ?? ""));
            identity.AddClaim(new Claim("lastname", user.LastName ?? ""));
            return identity;
        }
    }
}
