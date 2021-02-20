using Shared.Resources.PermissionCategory;
using System.Collections.Generic;

namespace Shared.Resources.Role
{
    public class RoleGetData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DateCreated { get; set; }
        public string Description { get; set; }
        public List<PermissionCategoryRelationGetData> Permissions { get; set; }
        public bool IsEditable { get; set; }
    }
}
