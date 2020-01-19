using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeneralHelper.Lib.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<StudentRecord> StudentRecord { get; set; }
    }


    public partial class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Value { get; set; }
        public DateTime TokenExpiration { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser AspNetUsers { get; set; }
    }

     public partial class StudentRecord
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser AspNetUsers { get; set; }
    }
}
