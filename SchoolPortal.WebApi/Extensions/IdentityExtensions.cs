using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SchoolPortal.WebApi
{
    public static class IdentityExtensions
    {
        public static int GetUserId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("UserId");
            return (claim != null) ? int.Parse(claim.Value) : 0;
        }

        public static string GetCompanyId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("role");
            return (claim != null) ? claim.Value : "";
        }
        public static string GetFirstName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FirstName");
            return (claim != null) ? claim.Value : "";
        }
        public static string GetLastName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("LastName");
            return (claim != null) ? claim.Value : "";
        }

    }
}
