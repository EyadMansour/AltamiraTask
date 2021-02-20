using Data.Common;
using Data.Configurations;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Common.Enums;
using Domain.Entities.Companies;
using Domain.Entities.Addresses;
namespace Data
{

    public class ApplicationDbContext : IdentityDbContext<User,
        Role, string, UserClaim,
        UserRole,
        UserLogin,
        RoleClaim,
        UserToken>, IDbContext

    {

        #region Identity 
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<PermissionCategory> PermissionCategories { get; set; }
        public DbSet<PermissionCategoryPermission> PermissionCategoryPermissions { get; set; }
        public DbSet<RolePermissionCategory> RolePermissionCategories { get; set; }
        #endregion

        #region Entities
        
        

		public DbSet<Company> Companies { get; set; }
		public DbSet<Address> Addresss { get; set; }
        #endregion
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RolePermissionCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

            #region Entity Tables 
            modelBuilder.Entity<User>().ToTable("Users", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<UserDetail>().ToTable("UserDetails", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<Role>().ToTable("Roles", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<UserRole>().ToTable("UserRoles", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<UserToken>().ToTable("UserTokens", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<RolePermissionCategory>().ToTable("RolePermissionCategories", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<UserPermission>().ToTable("UserPermissions", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<Permission>().ToTable("Permissions", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<PermissionCategory>().ToTable("PermissionCategories", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<PermissionCategoryPermission>().ToTable("PermissionCategoryPermissions", SchemaNames.IdentityTableSchemaName);
            #endregion


            

			modelBuilder.Entity<Company>().ToTable("Companies");
			modelBuilder.Entity<Address>().ToTable("Addresss");
            modelBuilder.SetStatusQueryFilter();

        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["DateCreated"] = DateTime.Now;
                        entry.CurrentValues["Status"] = RecordStatus.Active;
                        break;
                    case EntityState.Modified:
                        entry.CurrentValues["DateModified"] = DateTime.Now;
                        break;
                }
        }
    }
}
