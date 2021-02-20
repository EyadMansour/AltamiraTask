using Core.Utilities;
using Data.Initialize.Data;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Initialize
{
    public static class Initialize
    {

        public static async Task SeedAsync(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;

            await context.Database.MigrateAsync();
            await context.Database.EnsureCreatedAsync();

            #region Entity Seed

            await SeedCompanyAsync(context).ConfigureAwait(false);
            #endregion

            #region Identity Seed
            await SeedPermissionAsync(context).ConfigureAwait(false);
            await SeedPermissionCategoryAsync(context).ConfigureAwait(false);

            await SeedUserDataAsync(userManager,context).ConfigureAwait(false);
            await SeedRoleDataAsync(roleManager).ConfigureAwait(false);
            await JoinUserRoleAsync(userManager, context).ConfigureAwait(false);
            await JoinRolePermissionAsync(roleManager, context).ConfigureAwait(false);
            JoinUserDirectivePermission(userManager, context);
            await context.SaveChangesAsync().ConfigureAwait(false);



            #endregion


            


        }



        private static async Task SeedPermissionAsync(ApplicationDbContext context)
        {
            var data = InitializeData.BuildPermissionsList();
            var dbData = context.Permissions.ToList();
            var diffData = data.Where(c => dbData.All(e => e.Label != c.Label)).ToList();
            await context.Permissions.AddRangeAsync(diffData).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
        
        private static async Task SeedCompanyAsync(ApplicationDbContext context)
        {
            var data =await InitializeData.BuildCompanyList();
            var dbData = context.Companies.ToList();
            var diffData = data.Where(c => dbData.All(e => e.Name != c.Name)).ToList();
            await context.Companies.AddRangeAsync(diffData).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
        private static async Task SeedPermissionCategoryAsync(ApplicationDbContext context)
        {
            var data = InitializeData.BuildPermissionCategories(context);
            var dbData = context.PermissionCategories;
            var diffData = data.Where(c => dbData.All(e => e.Label != c.Label)).ToList();
            await context.PermissionCategories.AddRangeAsync(diffData).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
        private static async Task SeedUserDataAsync(UserManager<User> userManager, ApplicationDbContext context)
        {
            //şifre random oluşturabilirsiniz alttaki yorum satrı kullanarak.
            var defaultPassword = "Aa12345!";
            //var defaultPassword = StringGenerator.GeneratePassword(8, true, true, true, true);
            
            var data =await InitializeData.BuildUserList(context);
            var dbData = userManager.Users.ToList();
            var diffData = data.Where(c => dbData.All(e => e.UserName != c.UserName)).ToList();
            foreach (var user in diffData)
            {
                await userManager.CreateAsync(user, defaultPassword).ConfigureAwait(false);

            }
        }
        private static async Task SeedRoleDataAsync(RoleManager<Role> roleManager)
        {
            var data = InitializeData.BuildRoleList();
            var dbData = roleManager.Roles.ToList();
            var diffData = data.Where(c => dbData.All(e => e.Name != c.Name)).ToList();
            foreach (var role in diffData)
            {
                await roleManager.CreateAsync(role).ConfigureAwait(false);

            }
        }
        private static async Task JoinUserRoleAsync(UserManager<User> userManager, ApplicationDbContext context)
        {
            //if (userManager.Users.Any() && !context.UserRoles.Any())
            //{
            //    await userManager.AddToRoleAsync(await userManager.FindByNameAsync("admin").ConfigureAwait(false), "ADMIN").ConfigureAwait(false);
            //}

        }
        private static async Task JoinRolePermissionAsync(RoleManager<Role> roleManager, ApplicationDbContext context)
        {
            if (roleManager.Roles.Any())
            {
                var adminRole = await roleManager.Roles.Include(c => c.PermissionCategory).ThenInclude(c => c.PermissionCategoryPermission).FirstOrDefaultAsync(c => c.Name == "ADMIN").ConfigureAwait(false);

                var allPermission = context.PermissionCategoryPermissions.ToList();
                var dbData = adminRole.PermissionCategory.Select(c => c.PermissionCategoryPermission).ToList();
                var diffData = allPermission.Where(c => dbData.All(e => e.Id != c.Id)).ToList();
                foreach (var permission in diffData)
                {
                    adminRole.PermissionCategory.Add(new RolePermissionCategory()
                    {
                        PermissionCategoryPermission = permission
                    });
                }
            }

        }
        private static void JoinUserDirectivePermission(UserManager<User> userManager, ApplicationDbContext context)
        {
            //if (userManager.Users.Any())
            //{
            //    var directivePermissions = InitializeData.BuildUserDirectivePermissionsList(context);

            //    var adminArr = new string[] { "admin", "timAdmin" };
            //    var admins = userManager.Users.Include(c => c.DirectivePermissions)
            //        .Where(c => adminArr.Contains(c.UserName));
            //    foreach (var admin in admins)
            //    {
            //        var dbData = admin.DirectivePermissions;
            //        var diffData = directivePermissions.Where(c => dbData.All(e => e.PermissionId != c.PermissionId)).ToList();
            //        foreach (var permission in diffData)
            //        {
            //            admin.DirectivePermissions.Add(permission);
            //        }
            //    }
            //}

        }
    }
}
