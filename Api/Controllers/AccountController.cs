using Application.Services;
using AutoMapper;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Core.Constants;
using Core.Extensions;
using Core.Utilities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Resources.User;
using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Entities.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IUserService userService, SignInManager<User> signInManager, IMapper mapper)
        {
            _userService = userService;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        [HttpPost("Login")]
        public async Task<ApiResponse> Login([FromBody] UserLoginData data)
        {

            var isEmail = data.EmailOrUsername.IsEmail();
            var isUsername = data.EmailOrUsername.IsUsername();

            if (!(isEmail || isUsername))
                ModelState.AddModelError("emailOrUsername", "Enter valid Email/Username");

            if (!ModelState.IsValid)
                throw new ApiException(ModelState.AllErrors());

            var userQuery = _userService.GetAll();

            User user;
            switch (isUsername)
            {
                case true:
                    user = await userQuery.FirstOrDefaultAsync(c => c.UserName == data.EmailOrUsername)
                        .ConfigureAwait(false);
                    break;
                case false:
                    user = await userQuery.FirstOrDefaultAsync(c => c.Email == data.EmailOrUsername)
                        .ConfigureAwait(false);
                    break;
            }

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, data.Password, false, false).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    var mapResult = _mapper.Map<User, UserGetData>(user);

                    return new ApiResponse(new UserLoginResponse
                    {
                        User = mapResult,
                    });
                }
            }
            throw new ApiException(MessageBuilder.LoginFault);
        }


        
        [Authorize]
        [HttpPost("Logout")]
        public async Task<ApiResponse> Logout()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(false);
            return new ApiResponse(MessageBuilder.Logout, StatusCodes.Status200OK);
        }
    }
}
