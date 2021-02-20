using Domain.Common.Configurations;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Domain.Entities.Identity
{

    public class User : IdentityUser<string>, IEntity
    {

        public ICollection<UserRole> Roles { get; set; } = new Collection<UserRole>();
        public ICollection<UserPermission> DirectivePermissions { get; set; } = new Collection<UserPermission>();
        public UserDetail Detail { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public RecordStatus Status { get; set; } = RecordStatus.Active;
    }
}
