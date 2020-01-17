using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolPortal.Models.DataModels
{
    public class AspNetUserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }
        public bool MustChangePassword { get; set; }

        public bool LockoutEnabled { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateRegistered { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }

        public bool IsBusinessAccountApproved { get; set; }
    }
    public partial class AspNetUserItem : AspNetUserModel
    {



    }
    public partial class UserRegistrationModel : AspNetUserModel
    {


    }
    public class AspNetUserDetails : AspNetUserItem
    {

    }
    public partial class AspNetUserFilter : AspNetUserItem
    {

        public static AspNetUserFilter Deserilize(string whereCondition)
        {
            AspNetUserFilter filter = null;
            if (whereCondition != null)
            {
                filter = JsonConvert.DeserializeObject<AspNetUserFilter>(whereCondition);
            }
            return filter;
        }
    }
}
