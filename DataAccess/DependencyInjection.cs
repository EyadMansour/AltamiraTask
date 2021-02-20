using DataAccess.Repository;
using Domain.Entities.Identity;
using Microsoft.Extensions.DependencyInjection;

using DataAccess.Repository.Companies;
using DataAccess.Repository.Addresses;
using DataAccess.Repository.Addresses;

namespace DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddRepositories();


            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {


			services.AddScoped<ICompanyRepository, CompanyRepository>();
			services.AddScoped<IAddressRepository, AddressRepository>();

            #region Identity ve configlar 
            services.AddScoped<IRepository<UserRole>, Repository<UserRole>>();
            services.AddScoped<IRepository<Permission>, Repository<Permission>>();
            services.AddScoped<IRepository<PermissionCategoryPermission>, Repository<PermissionCategoryPermission>>();
            services.AddScoped<IRepository<UserPermission>, Repository<UserPermission>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            return services;
        }
    }
}
