using Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace Shared.Resources.User
{
    public class UserRegisterData
    {
        [Required]
        [RegularExpression(
            RegexConstants.EmailRegex,
            ErrorMessage = "Email is not in valid format.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(RegexConstants.UserNameRegex, ErrorMessage = "Username is not in valid format.")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(RegexConstants.PasswordRegex, ErrorMessage = "Password is not in valid format.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [StringLength(StringLengths.LengthXs)]
        public string Name { get; set; }
        //[Required]
        [StringLength(StringLengths.LengthXs)]
        public string SurName { get; set; }
        [StringLength(StringLengths.LengthXxxs)]
        public string Phone { get; set; }

    }
}
