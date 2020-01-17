
using System.ComponentModel.DataAnnotations;


namespace GeneralHelper.Lib.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    
    public class ForgotPasswordModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }

    public class ResetPasswordModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }

    }
    public class AddPasswordModel
    {

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

    }
    public class ConfirmEmailModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
    public class ConfirmEmailData
    {

        public string UserId { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RegisterViewModel
    {
        
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
    }

    public class ChangePasswordModel
    {


        public string OldPassword { get; set; }


        public string NewPassword { get; set; }


        public string ConfirmPassword { get; set; }

    }
}
