using Core.Constants;
using Domain.Common.Configurations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Identity
{
    public class PermissionCategory : BaseEntity, IEntity
    {
        [Key]
        [Required]
        [StringLength(StringLengths.LengthSm)]
        public string Label { get; set; }
        [StringLength(StringLengths.LengthSm)]
        public string VisibleLabel { get; set; }
        [StringLength(StringLengths.LengthMd)]
        public string Description { get; set; }

        public ICollection<PermissionCategoryPermission> PossiblePermissions { get; set; } = new Collection<PermissionCategoryPermission>();

    }
}
