using Domain.Common.Enums;
using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Initialize.Data
{
    public static partial class InitializeData
    {

        public static List<PermissionCategory> BuildPermissionCategories(ApplicationDbContext context)
        {
            var permissions = context.Permissions.ToList();
            var add = permissions.First(c => c.Label == nameof(PermissionEnum.Add));
            var edit = permissions.First(c => c.Label == nameof(PermissionEnum.Edit));
            var delete = permissions.First(c => c.Label == nameof(PermissionEnum.Delete));
            var list = permissions.First(c => c.Label == nameof(PermissionEnum.List));
            var now = DateTime.Now;
            List<PermissionCategory> categories = new List<PermissionCategory>()
                {
                    new PermissionCategory()
                {
                    Label = "User",
                    VisibleLabel = "User",
                    DateCreated = now ,
                    Description = "",
                    Status = RecordStatus.Active,
                    PossiblePermissions = new List<PermissionCategoryPermission>()
                    {
                        new PermissionCategoryPermission()
                        {
                            Permission = add,
                            DateCreated = now ,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission = edit,
                            DateCreated = now ,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission =delete,
                            DateCreated = now ,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission = list,
                            DateCreated = now ,

                        },

                    }

                },
                new PermissionCategory()
                {
                    Label = "Role",
                    VisibleLabel = "Role",
                    DateCreated = now ,
                    Description = "",
                    Status = RecordStatus.Active,
                    PossiblePermissions = new List<PermissionCategoryPermission>()
                    {
                        new PermissionCategoryPermission()
                        {
                            Permission = add,
                            DateCreated = now ,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission = list ,
                            DateCreated = now ,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission = edit,
                            DateCreated = now ,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission = delete,
                            DateCreated = now ,

                        },

                    }

                },
                new PermissionCategory()
                {
                    Label = "Company",
                    VisibleLabel = "Company",
                    DateCreated = now ,
                    Description = "",
                    Status = RecordStatus.Active,
                    PossiblePermissions = new List<PermissionCategoryPermission>()
                    {
                        new PermissionCategoryPermission()
                        {
                            Permission = add,
                            DateCreated = now ,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission = edit,
                            DateCreated = now ,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission =delete,
                            DateCreated = now ,

                        },
                        new PermissionCategoryPermission()
                        {
                            Permission = list,
                            DateCreated = now ,

                        },

                    }

                },

            };



            return categories;
        }


    }
}
