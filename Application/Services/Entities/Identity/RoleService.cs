using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess;
using Domain.Entities.Identity;

namespace Application.Services
{
    public class RoleService
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }


        public Task<Role> GetRoleByIdAsync(string roleId)
        {
            return _roleManager.FindByIdAsync(roleId);
        }

        public Task<Role> GetRoleByIdAsync(string roleId, params Expression<Func<Role, object>>[] includeProperties)
        {
            var query = _roleManager.Roles.IncludeAll(includeProperties);
            return query.FirstOrDefaultAsync(c => c.Id == roleId);
        }


        public Task<Role> GetRoleByIdAsync(string roleId, params string[] includeProperties)
        {
            var query = _roleManager.Roles.IncludeAll(includeProperties);

            return query.FirstOrDefaultAsync(c => c.Id == roleId);
        }
        public IQueryable<Role> FindBy(Expression<Func<Role, bool>> predicate)
        {
            return _roleManager.Roles.Where(predicate);
        }
        public IQueryable<Role> FindBy(Expression<Func<Role, bool>> predicate, params string[] includeProperties)
        {
            return _roleManager.Roles.IncludeAll(includeProperties).Where(predicate);
        }
        public IQueryable<Role> FindBy(Expression<Func<Role, bool>> predicate, params Expression<Func<Role, object>>[] includeProperties)
        {
            return _roleManager.Roles.IncludeAll(includeProperties).Where(predicate);
        }

        public IQueryable<Role> GetAll()
        {
            return _roleManager.Roles;
        }
        public IQueryable<Role> GetAll(params string[] includeProperties)
        {
            return _roleManager.Roles.IncludeAll(includeProperties);
        }
        public IQueryable<Role> GetAll(params Expression<Func<Role, object>>[] includeProperties)
        {
            return _roleManager.Roles.IncludeAll(includeProperties);
        }

        public Task<bool> IsExistAsync(Expression<Func<Role, bool>> predicate)
        {
            return _roleManager.Roles.AnyAsync(predicate);
        }
        public Task<IdentityResult> CreateAsync(Role role)
        {
            return _roleManager.CreateAsync(role);
        }
        public Task<IdentityResult> UpdateAsync(Role role)
        {
            return _roleManager.UpdateAsync(role);
        }

        public Task<IdentityResult> DeleteAsync(Role role)
        {
            return _roleManager.DeleteAsync(role);
        }

    }
}
