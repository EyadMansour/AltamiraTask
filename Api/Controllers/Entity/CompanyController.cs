using Core.Exceptions;
using Application.Services.Entities.Companies;
using Shared.Resources.Companies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Cache;
using Core.Utilities.Helpers.DataTable;
using Core.Utilities.Helpers.Cryptograpies;

namespace Api.Controllers.Entity
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _service;
        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _service.ListAsync().ConfigureAwait(false);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            
            var result = await _service.GetAsync(id).ConfigureAwait(false);

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CompanyData companyData)
        {
            await _service.CreateAsync(companyData).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(CompanyData companyData)
        {
            var result = await _service.UpdateAsync(companyData).ConfigureAwait(false);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
           await _service.DeleteAsync(id).ConfigureAwait(false);
            return Ok();
        }
    }
}
