using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

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

        public string SecurityToken { get; set; }

        public ICollection<StudentRecord> StudentRecords { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
