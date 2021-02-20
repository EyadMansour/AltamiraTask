using Domain.Common.Configurations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    public class RolePermissionCategory : BaseEntity, IEntity
    {
        [ForeignKey("PermissionCategoryPermission")]
        public int PermissionCategoryPermissionId { get; set; }
        [ForeignKey("Role")]
        public string RoleId { get; set; }
        public PermissionCategoryPermission PermissionCategoryPermission { get; set; }
        public Role Role { get; set; }
    }
}
