using Core.Constants;
using Domain.Common.Configurations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Identity
{
    public class Permission : BaseEntity, IEntity
    {
        [Key]
        [Required]
        [StringLength(StringLengths.LengthSm)]
        public string Label { get; set; }

        [StringLength(StringLengths.LengthSm)]
        public string VisibleLabel { get; set; }

        [StringLength(StringLengths.LengthMd)]
        public string Description { get; set; }
        public bool IsDirective { get; set; }

        public ICollection<PermissionCategoryPermission> Categories { get; set; }
        public Permission() { Categories = new Collection<PermissionCategoryPermission>(); }
    }
}
