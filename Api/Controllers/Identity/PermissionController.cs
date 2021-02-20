using AutoMapper;
using AutoWrapper.Wrappers;
using DataAccess.Repository;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Resources.Permission;
using Shared.Resources.PermissionCategory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Controllers.Api.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<PermissionCategoryPermission> _repository;
        private readonly IRepository<Permission> _permissionRepository;

        public PermissionController(IRepository<PermissionCategoryPermission> repository, IMapper mapper, IRepository<Permission> permissionRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _permissionRepository = permissionRepository;
        }


        [HttpGet]
        public async Task<ApiResponse> GetAllAsync()
        {
            var data = _repository.GetAll("Permission", "Category");
            var result =
                _mapper.Map<List<PermissionCategoryPermission>, List<PermissionCategoryRelationGetData>>(await data.ToListAsync()
                    .ConfigureAwait(false));
            return new ApiResponse(result, StatusCodes.Status200OK);
        }


        [HttpGet("GetAllDirectivePermissions")]
        public async Task<ApiResponse> GetAllDirectivePermissionsAsync()
        {
            var data = _permissionRepository.FindBy(c => c.IsDirective);
            var result =
                _mapper.Map<List<Permission>, List<PermissionGetData>>(await data.ToListAsync()
                    .ConfigureAwait(false));
            return new ApiResponse(result, StatusCodes.Status200OK);
        }

    }
}
