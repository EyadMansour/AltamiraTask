using Domain.Common.Configurations;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities.Identity
{
    public class UserRole : IdentityUserRole<string>, IEntity
    {

        public User User { get; set; }
        public Role Role { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public RecordStatus Status { get; set; } = RecordStatus.Active;
    }

}
