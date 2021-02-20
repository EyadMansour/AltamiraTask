using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Utilities.Attributes
{

    public class MustBeGuidAttribute : ValidationAttribute
    {
        private readonly string[] _formats;

        public MustBeGuidAttribute(params string[] formats)
        {
            _formats = formats;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {


            if (value is string s)
            {

                Guid guid;
                var isParsed = Guid.TryParse(s, out guid);
                if (isParsed)
                    return ValidationResult.Success;


            }
            return new ValidationResult(!string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : "Must be in guid format!");


        }

    }
}
