﻿using aspnetcore6.ntier.API.Requests;
using aspnetcore6.ntier.API.Responses;
using aspnetcore6.ntier.Services.DTO.AccessControl;
using aspnetcore6.ntier.Services.DTO.Shared;
using aspnetcore6.ntier.Services.Interfaces.AccessControl;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore6.ntier.API.Controllers.AccessControl
{
    [ApiController]
    [Route("api/permission")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiDataResponse<IEnumerable<PermissionDTO>>>> GetPermissions()
        {
            IEnumerable<PermissionDTO> permissions = await _permissionService.GetPermissions();
            var response = new ApiDataResponse<IEnumerable<PermissionDTO>>(permissions, "Permissions retrieved succcessfully.");
            return Ok(response);
        }


        [HttpGet("paginated")]
        public async Task<ActionResult<ApiPagnatedResponse<PermissionDTO>>> GetPaginatedPermissions([FromQuery] PaginateQueryParameters qp)
        {
            PaginatedDataDTO<PermissionDTO> pp = await _permissionService.GetPaginatedPermissions(
                qp.PageNumber,
                qp.PageSize,
                qp.searchText,
                qp.orderByProperty,
                qp.ascending);

            var response = new ApiPagnatedResponse<PermissionDTO>(
                pp.Data,
                pp.PageNumber,
                pp.TotalPages,
                pp.PageSize,
                pp.TotalCount,
                pp.HasPrevious,
                pp.HasNext,
                "Permissions retrieved succcessfully.");

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiDataResponse<PermissionDTO>>> GetPermission(int id)
        {
            PermissionDTO permission = await _permissionService.GetPermission(id);
            var response = new ApiDataResponse<PermissionDTO>(permission, "Permission retrieved succcessfully.");
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<ApiBaseResponse>> PostPermission(AddPermissionDTO permissionDTO)
        {
            await _permissionService.AddPermission(permissionDTO);
            var response = new ApiBaseResponse("Permission created succcessfully.");
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ApiBaseResponse>> PutPermission(UpdatePermissionDTO permissionDTO)
        {
            await _permissionService.UpdatePermission(permissionDTO);
            var response = new ApiBaseResponse("Permission updated succcessfully.");
            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ApiBaseResponse>> DeletePermission(int id)
        {
            await _permissionService.DeletePermission(id);
            var response = new ApiBaseResponse("Permission deleted succcessfully.");
            return Ok(response);
        }
    }
}
