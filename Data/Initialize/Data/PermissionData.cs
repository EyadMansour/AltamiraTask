using Domain.Common.Enums;
using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Initialize.Data
{
    public static partial class InitializeData
    {

        private static string GetPermissionVisibleName(string permission)
        {
            var visibleName = permission switch
            {
                nameof(PermissionEnum.Add) => "Add",
                nameof(PermissionEnum.Edit) => "Edit",
                nameof(PermissionEnum.Delete) => "Delete",
                nameof(PermissionEnum.List) => "List",
                _ => ""
            };
            return visibleName;
        }
        public static List<Permission> BuildPermissionsList()
        {
            var list = new List<Permission>();
            var directivePermissions = new List<string>()
            {
                nameof(PermissionEnum.Admin),nameof( PermissionEnum.Moderator)
            };
            foreach (var permission in Enum.GetNames(typeof(PermissionEnum)))
            {
                list.Add(new Permission()
                {
                    Label = permission,
                    VisibleLabel = GetPermissionVisibleName(permission),
                    DateCreated = DateTime.Now,
                    Description = "",
                    Status = RecordStatus.Active,
                    IsDirective = directivePermissions.Any(c => c == permission)
                });
            }

            return list;
        }

    }
}
