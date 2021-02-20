using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Cache;
using Application.Services.Entities.Companies;
using Application.Services.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Shared.Resources.Companies;
using Shared.Resources.User;

namespace Api.Controllers.Entity
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = _service.GetAll("Detail.Company", "Detail.Address");
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _service.GetUserDetailsByIdAsync(id, "Detail.Company", "Detail.Address").ConfigureAwait(false);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserDetailsData userData)
        {
            await _service.CreateAsync(userData).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(UserDetailEditData userData)
        {
            await _service.UpdateAsync(userData).ConfigureAwait(false);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id).ConfigureAwait(false);
            return Ok();
        }
    }
}
