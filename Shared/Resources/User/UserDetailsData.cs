using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Constants;
using Shared.Resources.Addresses;

namespace Shared.Resources.User
{
    public class UserDetailsData: UserDetailSharedInputData
    {
        [Required]
        [RegularExpression(RegexConstants.PasswordRegex, ErrorMessage = "Password is not in valid format.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
