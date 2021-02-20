using Castle.Core.Internal;
using Core.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Services.Cache;
using Application.Services.Entities.Identity;
using Application.Validators.Users;
using DataAccess;
using Domain.Common.Enums;
using Domain.Entities.Identity;
using Shared.Resources.User;
using Core.Exceptions;
using AutoMapper;
using DataAccess.Repository.Companies;

namespace Application.Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ICompanyRepository _companyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CreateUserValidator _createValidationRules;
        private readonly UserPasswordValidator _passwordValidationRules;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        private readonly string _userListCacheKey;
        public UserService(IMapper mapper, ICompanyRepository repository,UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, ICacheService cache)
        {
            _userManager = userManager;
            _companyRepository = repository;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _createValidationRules = new CreateUserValidator();
            _passwordValidationRules=new UserPasswordValidator();
            _mapper = mapper;
            _userListCacheKey = "userList";
        }

        public string GetAuthorizedUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public string GetAuthorizedUserId(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        public string GetAuthorizedUserName(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Name);
        }

        public Task<User> GetUserByNameAsync(string userName)
        {
            if (userName.IsNullOrEmpty())
                return Task.FromResult<User>(null);
            return _userManager.FindByNameAsync(userName);
        }
        public Task<User> GetUserByNameAsync(string userName, params Expression<Func<User, object>>[] includeProperties)
        {
            if (userName.IsNullOrEmpty())
                return Task.FromResult<User>(null);
            var query = _userManager.Users.IncludeAll(includeProperties);
            return query.FirstOrDefaultAsync(c => c.UserName == userName);
        }
        public Task<User> GetUserByNameAsync(string userName, params string[] includeProperties)
        {
            if (userName.IsNullOrEmpty())
                return Task.FromResult<User>(null);
            var query = _userManager.Users.IncludeAll(includeProperties);
            return query.FirstOrDefaultAsync(c => c.UserName == userName);
        }

        public Task<User> GetUserByIdAsync(string userId)
        {
            return _userManager.FindByIdAsync(userId);
        }

        public Task<User> GetUserByIdAsync(string userId, params Expression<Func<User, object>>[] includeProperties)
        {
            var query = _userManager.Users.IncludeAll(includeProperties);
            return query.FirstOrDefaultAsync(c => c.Id == userId);
        }

        public async Task<UserGetData> GetUserDetailsByIdAsync(string userId, params string[] includeProperties)
        {
            var query = _userManager.Users.IncludeAll(includeProperties);
            var user = await query.FirstOrDefaultAsync(c => c.Id == userId);
            return _mapper.Map<UserGetData>(user);
        }
        public Task<User> GetUserByIdAsync(string userId, params string[] includeProperties)
        {
            var query = _userManager.Users.IncludeAll(includeProperties);

            return query.FirstOrDefaultAsync(c => c.Id == userId);
        }

        public IQueryable<User> FindBy(Expression<Func<User, bool>> predicate)
        {
            return _userManager.Users.Where(predicate);
        }
        public IQueryable<User> FindBy(Expression<Func<User, bool>> predicate, params string[] includeProperties)
        {
            return _userManager.Users.IncludeAll(includeProperties).Where(predicate);
        }
        public IQueryable<User> FindBy(Expression<Func<User, bool>> predicate, params Expression<Func<User, object>>[] includeProperties)
        {
            return _userManager.Users.IncludeAll(includeProperties).Where(predicate);
        }
        public IQueryable<User> GetAll()
        {
            return _userManager.Users;
        }
        public List<UserGetData> GetAll(params string[] includeProperties)
        {
            var cacheList = _cache.GetCacheValue<List<UserGetData>>(_userListCacheKey);
            if (cacheList != null)
            {
                return cacheList;
            }

            var list = _userManager.Users.IncludeAll(includeProperties);
            var mappedList = _mapper.Map<List<UserGetData>>(list.Where(x=>x.Status==RecordStatus.Active).ToList());
            _cache.SetCacheValueAsync<List<UserGetData>>(_userListCacheKey, mappedList);
            return mappedList;
            
        }
        public IQueryable<User> GetAll(params Expression<Func<User, object>>[] includeProperties)
        {
            return _userManager.Users.IncludeAll(includeProperties);
        }



        public Task<bool> IsExistAsync(Expression<Func<User, bool>> predicate)
        {
            return _userManager.Users.AnyAsync(predicate);
        }
        public async Task CreateAsync(UserDetailsData userData)
        {
            await CheckRecordValidation(userData);
            await CheckCreateModel(userData);
            
            var user = _mapper.Map<UserDetailsData, User>(userData);
            if (user == null)
            {
                throw new NullReferenceException();
            }
            user.Id = Guid.NewGuid().ToString();
            user.EmailConfirmed = true;
            IdentityResult result = await _userManager.CreateAsync(user, userData.Password);
            if (!result.Succeeded)
            {
                var errorMessage = result.Errors.FirstOrDefault();
                throw new InvalidDataException();
            }

            await _cache.DeleteCacheValue(_userListCacheKey);
        }

        public async Task CheckCreateModel(UserDetailsData userData)
        {
            var validate = await _passwordValidationRules.ValidateAsync(userData).ConfigureAwait(false);
            if (!validate.IsValid)
            {
                throw new EntityValidationException(validate.ToString("~"));
            }

            if (await IsExistAsync(c => c.UserName == userData.UserName || c.Email == userData.Email)
                .ConfigureAwait(false))
            {
                throw new RecordAlreadyExistException();
            }
        }
        public async Task CheckRecordValidation(UserDetailSharedInputData userData)
        {
            var validate = await _createValidationRules.ValidateAsync(userData).ConfigureAwait(false);
            if (!validate.IsValid)
            {
                throw new EntityValidationException(validate.ToString("~"));
            }

            if (userData.CompanyId != null && !await _companyRepository.IsExistAsync(x => x.Id == userData.CompanyId)
                .ConfigureAwait(false))
            {
                throw new RecordNotFoundException("Company not found");
            }
        }
        public async Task UpdateAsync(UserDetailEditData userData)
        {
            await CheckRecordValidation(userData);
            
            var user = await GetUserByIdAsync(userData.Id, "Detail.Company", "Detail.Address").ConfigureAwait(false);
            if (user == null)
            {
                throw new NullReferenceException();
            }
            _mapper.Map<UserDetailEditData, User>(userData, user);
            await _userManager.UpdateAsync(user);
            
            await _cache.DeleteCacheValue(_userListCacheKey);
        }
        public Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }
        public async Task DeleteAsync(string id)
        {
            var user = await GetUserByIdAsync(id).ConfigureAwait(false);
            if (user == null)
            {
                throw new RecordNotFoundException();
            }
            user.Status = RecordStatus.Deleted;
            user.DateDeleted = DateTime.Now;
            await _userManager.UpdateAsync(user);
            await _cache.DeleteCacheValue(_userListCacheKey);
        }

        public bool IsAdmin(User user)
        {
            return UserIsInPermission(user, nameof(PermissionEnum.Admin));
        }
        public async Task<bool> IsAdminAsync()
        {
            var userId = GetAuthorizedUserId(_httpContextAccessor.HttpContext.User);
            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            return UserIsInPermission(await GetUserByIdAsync(userId, includeParams.ToArray()), nameof(PermissionEnum.Admin));
        }
        public async Task<bool> IsAdminAsync(string userId)

        {
            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            return UserIsInPermission(await GetUserByIdAsync(userId, includeParams.ToArray()), nameof(PermissionEnum.Admin));
        }
        public async Task<bool> UserIsInRoleAsync(User user, string roleName)
        {
            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            return roles.Any(s => s.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        public bool UserIsInPermission(User user, string permissionName)
        {
            var directivePermissions = user.DirectivePermissions.Select(c => c.Permission.Label).ToList();
            var userRole = user.Roles.Select(c => c.Role).ToList();
            var permissions = userRole.SelectMany(c => c.PermissionCategory.Select(e => e.PermissionCategoryPermission.Permission.Label)).ToList();

            return directivePermissions.Any(c => c.Equals(permissionName, StringComparison.OrdinalIgnoreCase)) ||
                   permissions.Any(c => c.Equals(permissionName, StringComparison.OrdinalIgnoreCase));


        }
        public async Task<bool> UserIsInPermissionAsync(string userId, string permissionName)
        {
            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            var user = await GetUserByIdAsync(userId, includeParams.ToArray()).ConfigureAwait(false);
            var directivePermissions = user.DirectivePermissions.Select(c => c.Permission.Label).ToList();
            var userRole = user.Roles.Select(c => c.Role).ToList();
            var permissions = userRole.SelectMany(c => c.PermissionCategory.Select(e => $"{ e.PermissionCategoryPermission.Category.Label.ToLower()}_{ e.PermissionCategoryPermission.Permission.Label.ToLower()}")).ToList();

            return directivePermissions.Any(c => c.Equals(permissionName, StringComparison.OrdinalIgnoreCase)) ||
                   permissions.Any(c => c.Equals(permissionName, StringComparison.OrdinalIgnoreCase));
        }



    }
}
