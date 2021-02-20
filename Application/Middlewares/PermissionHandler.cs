﻿using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Entities.Identity;
using Domain.Entities.Identity;

namespace Application.Middlewares
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public readonly PermissionRequirementModel[] Permission;

        public PermissionRequirement(params PermissionRequirementModel[] permission)
        {
            Permission = permission;
        }
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserService _userService;

        public PermissionHandler(IUserService userService)
        {

            _userService = userService;

        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = _userService.GetAuthorizedUserId(context.User);

            var user = await _userService.GetUserByIdAsync(userId,
                "Roles.Role.PermissionCategory.PermissionCategoryPermission.Permission", "DirectivePermissions.Permission").ConfigureAwait(false);
            if (user != null)
            {
                var userRole = new List<Role>();
                foreach (var c in user.Roles) userRole.Add(c.Role);

                var rolePermissions = userRole.SelectMany(c => c.PermissionCategory.Select(m => m.PermissionCategoryPermission)).ToList();

                var directivePermissions = user.DirectivePermissions.Select(c => c.Permission);


                if (
                    directivePermissions.Any(dp => requirement.Permission.Any(req => req.IsEqual(dp.Label)))
                    ||
                    rolePermissions.Any(rp =>
                        requirement.Permission.Any(req => req.IsEqual(rp.PermissionId, rp.CategoryId)))
                )
                {
                    context.Succeed(requirement);
                }
            }

            await Task.CompletedTask.ConfigureAwait(false);

        }
    }

}
