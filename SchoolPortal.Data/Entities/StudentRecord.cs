using System;
using System.Collections.Generic;

namespace SchoolPortal.Data.Entities
{
    public partial class StudentRecord
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

        public virtual AspNetUsers CreatedByNavigation { get; set; }
    }
}
