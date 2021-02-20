using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Shared.Resources.User
{
    public class UserLoginData
    {

        [Required(ErrorMessage = "Email or Username is not defined")]
        public string EmailOrUsername { get; set; }

        [Required(ErrorMessage = "Password is not defined")]
        public string Password { get; set; }

    }
}
