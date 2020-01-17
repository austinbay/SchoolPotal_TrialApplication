using GeneralHelper.Lib.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GeneralHelper.Lib.Services
{
   
    public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public UserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
                          IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UserId", user.Id.ToString()));
            identity.AddClaim(new Claim("FirstName", user.FirstName ?? ""));
            identity.AddClaim(new Claim("LastName", user.LastName ?? ""));
            //identity.AddClaim(new Claim("CompanyId", user.CompanyId.ToString()));


            return identity;
        }
    }
}
