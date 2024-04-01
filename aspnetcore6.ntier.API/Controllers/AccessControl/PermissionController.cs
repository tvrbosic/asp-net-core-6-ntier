using aspnetcore6.ntier.API.Requests;
using aspnetcore6.ntier.API.Responses;
using aspnetcore6.ntier.BLL.DTOs.AccessControl;
using aspnetcore6.ntier.BLL.DTOs.Shared;
using aspnetcore6.ntier.BLL.Interfaces.AccessControl;
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
        public async Task<ActionResult<IEnumerable<PermissionDTO>>> GetPermissions()
        {
            IEnumerable<PermissionDTO> permissions = await _permissionService.GetPermissions();
            var response = new ApiDataResponse<IEnumerable<PermissionDTO>>(permissions, "Permissions retrieved succcessfully.");
            return Ok(response);
        }


        [HttpGet("paginated")]
        public ActionResult<IEnumerable<PermissionDTO>> GetPaginatedPermissions([FromQuery] PaginateQueryParameters qp)
        {
            PaginatedDataDTO<PermissionDTO> pp = _permissionService.GetPaginatedPermissions(
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
        public async Task<ActionResult<PermissionDTO>> GetPermission(int id)
        {
            PermissionDTO permission = await _permissionService.GetPermission(id);
            var response = new ApiDataResponse<PermissionDTO>(permission, "Permission retrieved succcessfully.");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> PostPermission(AddPermissionDTO permissionDTO)
        {
            await _permissionService.AddPermission(permissionDTO);
            var response = new ApiBaseResponse("Permission created succcessfully.");
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> PutPermission(UpdatePermissionDTO permissionDTO)
        {
            await _permissionService.UpdatePermission(permissionDTO);
            var response = new ApiBaseResponse("Permission updated succcessfully.");
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePermission(int id)
        {
            await _permissionService.DeletePermission(id);
            var response = new ApiBaseResponse("Permission deleted succcessfully.");
            return Ok(response);
        }
    }
}
