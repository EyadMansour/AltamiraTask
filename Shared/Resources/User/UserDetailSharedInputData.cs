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
    public class UserDetailSharedInputData
    {
        [Required]
        [RegularExpression(RegexConstants.EmailRegex, ErrorMessage = "Email is not in valid format.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(RegexConstants.UserNameRegex, ErrorMessage = "Username is not in valid format.")]
        public string UserName { get; set; }
        public int? CompanyId { get; set; }
        [StringLength(StringLengths.LengthXs)]
        public string Name { get; set; }
        //[Required]
        [StringLength(StringLengths.LengthXs)]
        public string SurName { get; set; }

        [StringLength(StringLengths.LengthXs)]
        public string Phone { get; set; }

        [StringLength(StringLengths.LengthXs)]
        public string Website { get; set; }
        public AddressData Address { get; set; }
    }
}
