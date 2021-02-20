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
    public class CreateUserValidator : AbstractValidator<UserDetailSharedInputData>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("Email cannot be empty or null");
            RuleFor(x => x.UserName).NotEmpty().NotNull().WithMessage("Username cannot be empty or null");
            RuleFor(x => x.Email).Matches(RegexConstants.EmailRegex).WithMessage("Email not valid");
            RuleFor(x => x.UserName).Matches(RegexConstants.UserNameRegex).WithMessage("UserName not valid");
            
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Name length cannot be more than 50 character");
            RuleFor(x => x.Phone).MaximumLength(50).WithMessage("Phone length cannot be more than 50 character");
            RuleFor(x => x.Website).MaximumLength(50).WithMessage("Website length cannot be more than 50 character");

            RuleFor(x => x.Address.Street).MaximumLength(200).WithMessage("Street length cannot be more than 200 character");
            RuleFor(x => x.Address.Suite).MaximumLength(200).WithMessage("Suite length cannot be more than 200 character");
            RuleFor(x => x.Address.City).MaximumLength(200).WithMessage("City length cannot be more than 200 character");
            RuleFor(x => x.Address.ZipCode).MaximumLength(50).WithMessage("Zipcode length cannot be more than 50 character");
            
            RuleFor(x => x.Address.Geo.Lat).Must(CheckDecimal).WithMessage("Lat valid değil");
            RuleFor(x => x.Address.Geo.Lng).Must(CheckDecimal).WithMessage("Lng valid değil");

        }

        
        
        private bool CheckDecimal(double? geoInfo)
        {
            if (geoInfo != null)
            {
                var re = new Regex(RegexConstants.DecimalGeoRegex);
                return re.IsMatch(geoInfo.ToString());
            }

            return true;
        }
    }
}
