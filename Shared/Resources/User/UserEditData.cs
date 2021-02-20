using Core.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Resources.User
{
    public class UserEditData
    {

        [StringLength(StringLengths.LengthXs, ErrorMessage = "Id is not in valid format.")]
        public string Id { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
        public List<string> DirectivePermissions { get; set; } = new List<string>();
        [StringLength(StringLengths.LengthXs)]
        public string Name { get; set; }
        //[Required]
        [StringLength(StringLengths.LengthXs)]
        public string SurName { get; set; }
        [StringLength(StringLengths.LengthXxxs)]
        public string Phone { get; set; }
        public string Adress { get; set; }

        [StringLength(StringLengths.LengthMd)]
        public string Departman { get; set; }
        //[Required]
        [StringLength(StringLengths.LengthMd)]
        public string Title { get; set; }
    }
}
