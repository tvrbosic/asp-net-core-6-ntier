using aspnetcore6.ntier.API.Requests;
using aspnetcore6.ntier.API.Responses;
using aspnetcore6.ntier.Services.DTO.AccessControl;
using aspnetcore6.ntier.Services.DTO.Shared;
using aspnetcore6.ntier.Services.Interfaces.AccessControl;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore6.ntier.API.Controllers.AccessControl
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            IEnumerable<RoleDTO> roles = await _roleService.GetRoles();
            var response = new ApiDataResponse<IEnumerable<RoleDTO>>(roles, "Roles retrieved succcessfully.");
            return Ok(response);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetPaginatedRoles([FromQuery] PaginateQueryParameters qp)
        {
            PaginatedDataDTO<RoleDTO> pr = await _roleService.GetPaginatedRoles(
                qp.PageNumber,
                qp.PageSize,
                qp.searchText,
                qp.orderByProperty,
                qp.ascending);

            var response = new ApiPagnatedResponse<RoleDTO>(
                pr.Data,
                pr.PageNumber,
                pr.TotalPages,
                pr.PageSize,
                pr.TotalCount,
                pr.HasPrevious,
                pr.HasNext,
                "Roles retrieved succcessfully.");

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRole(int id)
        {
            RoleDTO role = await _roleService.GetRole(id);
            var response = new ApiDataResponse<RoleDTO>(role, "Role retrieved succcessfully.");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostRole(AddRoleDTO roleDTO)
        {
            await _roleService.AddRole(roleDTO);
            var response = new ApiBaseResponse("Role created succcessfully.");
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> PutRole(UpdateRoleDTO roleDTO)
        {
            await _roleService.UpdateRole(roleDTO);
            var response = new ApiBaseResponse("Role updated succcessfully.");
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _roleService.DeleteRole(id);
            var response = new ApiBaseResponse("Role deleted succcessfully.");
            return Ok(response);
        }
    }
}
