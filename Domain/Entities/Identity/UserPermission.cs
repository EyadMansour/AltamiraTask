using Domain.Common.Configurations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Identity
{
    public class UserPermission : BaseEntity, IEntity
    {
        [ForeignKey("Role")]
        [Required]
        public string UserId { get; set; }
        [ForeignKey("Permission")]
        public string PermissionId { get; set; }
        public User User { get; set; }
        public Permission Permission { get; set; }
    }
}
