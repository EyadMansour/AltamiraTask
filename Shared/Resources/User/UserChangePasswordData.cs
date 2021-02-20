using System.ComponentModel.DataAnnotations;

namespace Shared.Resources.User
{
    public class UserChangePasswordData
    {

        [StringLength(128, ErrorMessage = "Id is not in valid format.")]
        public string Id { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,100}$",
            ErrorMessage = "Old Password is not in valid format.")]
        public string OldPassword { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,100}$",
            ErrorMessage = "Password is not in valid format.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


    }
}
