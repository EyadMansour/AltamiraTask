using Application.Services;
using AutoMapper;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Core.Constants;
using Core.Utilities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Resources.Role;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Controllers.Api.Identity
{

    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]

    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(RoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<ApiResponse> GetAsync(string id)
        {
            var includeParams = new IncludeStringConstants().RolePermissionIncludeList;
            includeParams.Add("Users.User");
            var role = await _roleService.GetRoleByIdAsync(id, includeParams.ToArray()).ConfigureAwait(false);
            if (role != null)
            {
                var data = _mapper.Map<Role, RoleGetData>(role);
                return new ApiResponse(data);
            }

            throw new ApiException(MessageBuilder.NotFound, StatusCodes.Status404NotFound);
        }

        [HttpGet]
        public async Task<ApiResponse> GetAllAsync()
        {
            var includeParams = new IncludeStringConstants().RolePermissionIncludeList;
            includeParams.Add("Users.User");
            var role = await _roleService.FindBy(c => c.IsEditable, includeParams.ToArray()).ToListAsync().ConfigureAwait(false);
            var data = _mapper.Map<List<Role>, List<RoleGetData>>(role);
            return new ApiResponse(data);
        }

        [HttpPost]
        public async Task<ApiResponse> CreateAsync([FromBody] RoleData data)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());


            if (await _roleService.IsExistAsync(c => c.Name == data.Name).ConfigureAwait(false))
                throw new ApiException(MessageBuilder.AlreadyExist(data.Name), StatusCodes.Status409Conflict);

            var role = _mapper.Map<RoleData, Role>(data);
            role.Id = Guid.NewGuid().ToString();
            role.DateCreated = DateTime.Now;
            await _roleService.CreateAsync(role).ConfigureAwait(false);

            return await GetAsync(role.Id).ConfigureAwait(false);
        }


        [HttpPut]
        public async Task<ApiResponse> UpdateAsync([FromBody] RoleData data)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            var role = await _roleService.GetRoleByIdAsync(data.Id, new IncludeStringConstants().RolePermissionIncludeList.ToArray()).ConfigureAwait(false);
            if (role == null)
                throw new ApiException(MessageBuilder.NotFound, StatusCodes.Status404NotFound);
            if (!role.IsEditable)
                throw new ApiException(MessageBuilder.NotEditable);

            if (data.Name != role.Name &&
                await _roleService.IsExistAsync(c => c.Name == data.Name).ConfigureAwait(false))
                throw new ApiException(MessageBuilder.AlreadyExist(data.Name), StatusCodes.Status409Conflict);


            //update
            _mapper.Map<RoleData, Role>(data, role);
            role.DateModified = DateTime.Now;
            await _roleService.UpdateAsync(role).ConfigureAwait(false);

            return await GetAsync(role.Id).ConfigureAwait(false);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(string id)
        {

            var role = await _roleService.GetRoleByIdAsync(id).ConfigureAwait(false);
            if (role == null)
                throw new ApiException(MessageBuilder.NotFound, StatusCodes.Status404NotFound);

            if (!role.IsEditable)
                throw new ApiException(MessageBuilder.NotEditable, StatusCodes.Status409Conflict);

            await _roleService.DeleteAsync(role).ConfigureAwait(false);
            return new ApiResponse(MessageBuilder.Deleted());


        }

    }
}
