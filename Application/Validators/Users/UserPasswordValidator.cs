using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Core.Constants;
using FluentValidation;
using Shared.Resources.User;

namespace Application.Validators.Users
{
    public class UserPasswordValidator : AbstractValidator<UserDetailsData>
    {
        public UserPasswordValidator()
        {
            RuleFor(x => x.Password).Must((User, Password) => CheckPassword(User.ConfirmPassword, Password)).WithMessage("password not valid");
        }
        private bool CheckPassword(string confirmPassword, string password)
        {
            if (!password.IsNullOrEmpty())
            {
                var re = new Regex(RegexConstants.PasswordRegex);
                return re.IsMatch(password) && confirmPassword == password;
            }
            return true;
        }
    }
}
