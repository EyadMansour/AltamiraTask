using Core.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Resources.Role
{
    public class RoleData 
    {

        public string Id { get; set; }
        [Required]
        [StringLength(StringLengths.LengthSm)]
        public string Name { get; set; }
        [StringLength(StringLengths.LengthLg)]
        public string Description { get; set; }
        public List<int> PermissionCategories { get; set; } = new List<int>();

    }
}
