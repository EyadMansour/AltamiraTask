using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Shared.Resources.User;

namespace Application.Services.Entities.Identity
{
    public interface IUserService
    {
        public string GetAuthorizedUserId();
        public string GetAuthorizedUserId(ClaimsPrincipal user);
        public string GetAuthorizedUserName(ClaimsPrincipal user);
        public Task<User> GetUserByNameAsync(string userName);

        public Task<User> GetUserByNameAsync(string userName,
            params Expression<Func<User, object>>[] includeProperties);

        public Task<User> GetUserByNameAsync(string userName, params string[] includeProperties);
        public Task<User> GetUserByIdAsync(string userId);
        public Task<User> GetUserByIdAsync(string userId, params Expression<Func<User, object>>[] includeProperties);
        public Task<UserGetData> GetUserDetailsByIdAsync(string userId, params string[] includeProperties);
        public Task<User> GetUserByIdAsync(string userId, params string[] includeProperties);
        public IQueryable<User> FindBy(Expression<Func<User, bool>> predicate);
        public IQueryable<User> FindBy(Expression<Func<User, bool>> predicate, params string[] includeProperties);

        public IQueryable<User> FindBy(Expression<Func<User, bool>> predicate,
            params Expression<Func<User, object>>[] includeProperties);

        public IQueryable<User> GetAll();
        public List<UserGetData> GetAll(params string[] includeProperties);
        public IQueryable<User> GetAll(params Expression<Func<User, object>>[] includeProperties);
        public Task<bool> IsExistAsync(Expression<Func<User, bool>> predicate);
        public Task CreateAsync(UserDetailsData userData);
        public Task UpdateAsync(UserDetailEditData userData);
        public Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        public Task DeleteAsync(string id);
        public bool IsAdmin(User user);
        public Task<bool> IsAdminAsync();
        public Task<bool> IsAdminAsync(string userId);
        public Task<bool> UserIsInRoleAsync(User user, string roleName);
        public bool UserIsInPermission(User user, string permissionName);
        public Task<bool> UserIsInPermissionAsync(string userId, string permissionName);

    }
}
