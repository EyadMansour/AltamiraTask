using Shared.Resources.Permission;

namespace Shared.Resources.PermissionCategory
{
    public class PermissionCategoryRelationGetData
    {
        public int RelationId { get; set; }
        public PermissionCategoryGetData Category { get; set; }
        public PermissionGetData Permission { get; set; }

    }
}
