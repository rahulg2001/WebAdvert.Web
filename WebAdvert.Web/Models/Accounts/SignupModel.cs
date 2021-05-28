using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Accounts
{
    public class SignupModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Password must be at least 6 characters long", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "password and its confirmation not matching")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Enable 2FA")]
        public bool Enable2FA { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(20, ErrorMessage = "valid PhoneNumber required")]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
