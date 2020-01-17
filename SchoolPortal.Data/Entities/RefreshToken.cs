using System;
using System.Collections.Generic;

namespace SchoolPortal.Data.Entities
{
    public partial class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Value { get; set; }
        public DateTime TokenExpiration { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
