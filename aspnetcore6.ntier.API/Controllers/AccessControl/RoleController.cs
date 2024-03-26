using aspnetcore6.ntier.BLL.Services.AccessControl.DTOs;
using aspnetcore6.ntier.BLL.Services.AccessControl.Interfaces;
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
            return roles == null ? NotFound() : Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRole(int id)
        {
            RoleDTO role = await _roleService.GetRole(id);
            return role == null ? NotFound() : Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> PostRole(AddRoleDTO roleDTO)
        {
            return await _roleService.AddRole(roleDTO) ? Ok() : BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> PutRole(UpdateRoleDTO roleDTO)
        {
            return await _roleService.UpdateRole(roleDTO) ? Ok() : BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(int id)
        {
            return await _roleService.DeleteRole(id) ? Ok() : BadRequest();
        }
    }
}
