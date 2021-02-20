using Application.Services;
using AutoMapper;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Core.Constants;
using Core.Utilities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Resources.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Entities.Identity;

namespace Presentation.Controllers.Api.Identity
{

    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = "Bearer")]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("GetInfo")]
        public async Task<ApiResponse> GetInfoAsync()
        {
            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            includeParams.Add("Detail");
            var item = await _userService.GetUserByNameAsync(User.Identity.Name, includeParams.ToArray()).ConfigureAwait(false);

            if (item != null)
                return new ApiResponse(_mapper.Map<User, UserGetData>(item), StatusCodes.Status200OK);
            throw new ApiException(MessageBuilder.NotFound, StatusCodes.Status404NotFound);

        }

        [HttpGet("{id}")]
        public async Task<ApiResponse> GetAsync(string id)
        {
            //var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            //includeParams.Add("Detail");
            var item = await _userService.GetUserByIdAsync(id, "Roles.Role", "Detail").ConfigureAwait(false);
            if (item != null)
                return new ApiResponse(_mapper.Map<User, UserGetData>(item), StatusCodes.Status200OK);
            throw new ApiException(MessageBuilder.NotFound, StatusCodes.Status404NotFound);

        }
        [HttpGet]
        public async Task<ApiResponse> GetAllAsync()
        {
            //var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            //includeParams.Add("Detail");
            var data = _userService.GetAll( "Roles.Role", "Detail");
            return new ApiResponse(_mapper.Map<List<User>, List<UserGetData>>(await data.ToListAsync().ConfigureAwait(false)), StatusCodes.Status200OK);

        }

        [HttpPost]
        public async Task<ApiResponse> CreateAsync([FromBody] UserAddData data)
        {

            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            if (!await _userService.IsExistAsync(c => c.UserName == data.UserName || c.Email == data.Email)
                .ConfigureAwait(false))
            {
                var user = _mapper.Map<UserAddData, User>(data);

                user.Id = Guid.NewGuid().ToString();
                user.EmailConfirmed = true;
                IdentityResult result = null;
                try
                {
                    result = await _userService.CreateAsync(user, data.Password).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                //var result = await _userService.CreateAsync(user, data.Password).ConfigureAwait(false);
                if (result.Succeeded)
                    return await GetAsync(user.Id).ConfigureAwait(false);
                var errorMessage = result.Errors.FirstOrDefault();
                throw new ApiException(errorMessage, StatusCodes.Status400BadRequest);
            }
            return new ApiResponse(MessageBuilder.UserOrEmailExist, StatusCodes.Status400BadRequest);

        }
        [HttpPut]
        public async Task<ApiResponse> UpdateAsync([FromBody] UserEditData data)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            var includeParams = new IncludeStringConstants().UserRolePermissionIncludeArray.ToList();
            var user = await _userService.GetUserByIdAsync(data.Id, includeParams.ToArray()).ConfigureAwait(false);

            if (user == null)
                throw new ApiException(MessageBuilder.NotFound, StatusCodes.Status404NotFound);
            
            //update
            _mapper.Map<UserEditData, User>(data, user);

            await _userService.UpdateAsync(user).ConfigureAwait(false);

            return await GetAsync(user.Id).ConfigureAwait(false);
        }


        [HttpPut("ChangePassword")]
        public async Task<ApiResponse> ChangePassword([FromBody] UserChangePasswordData data)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            var user = await _userService.GetUserByIdAsync(data.Id).ConfigureAwait(false);
            if (user == null)
                throw new ApiException(MessageBuilder.NotFound, StatusCodes.Status404NotFound);
            
            var changePasswordResult =
                await _userService.ChangePasswordAsync(user, data.OldPassword, data.Password).ConfigureAwait(false);
            if (!changePasswordResult.Succeeded)
            {
                var errorMessage = changePasswordResult.Errors.FirstOrDefault();
                var message = errorMessage == null ? MessageBuilder.Fail : errorMessage.Description;
                throw new ApiException(message, StatusCodes.Status400BadRequest);
            }
            return new ApiResponse(StatusCodes.Status200OK);

        }


        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(string id)
        {
            var user = await _userService.GetUserByIdAsync(id).ConfigureAwait(false);
            if (user != null)
            {
                
                await _userService.DeleteAsync(user).ConfigureAwait(false);
                return new ApiResponse(MessageBuilder.Deleted(), StatusCodes.Status200OK);

            }
            throw new ApiException(MessageBuilder.NotFound, StatusCodes.Status404NotFound);
        }

    }
}
