using Core.Constants;
using Domain.Common.Configurations;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Identity
{
    public class Role : IdentityRole<string>, IEntity
    {
        public ICollection<UserRole> Users { get; set; } = new Collection<UserRole>();
        public ICollection<RolePermissionCategory> PermissionCategory { get; set; } = new Collection<RolePermissionCategory>();
        [StringLength(StringLengths.LengthLg)]
        public string Description { get; set; }
        public bool IsEditable { get; set; } = true;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public RecordStatus Status { get; set; } = RecordStatus.Active;



    }
}
