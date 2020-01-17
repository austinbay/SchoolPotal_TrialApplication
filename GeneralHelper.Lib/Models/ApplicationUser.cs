using Microsoft.AspNetCore.Identity;
using System;


namespace GeneralHelper.Lib.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateRegistered { get; set; }

        public bool IsActive { get; set; }
    }
}
