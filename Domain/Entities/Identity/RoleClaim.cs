using Domain.Common.Configurations;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<string>, IEntity
    {
        public RecordStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
