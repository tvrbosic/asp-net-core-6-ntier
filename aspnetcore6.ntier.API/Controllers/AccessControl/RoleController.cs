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
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public ActionResult<RoleDTO> GetRole(int id)
        {
            RoleDTO role = _roleService.GetRole(id);
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> PostRole(AddRoleDTO roleDTO)
        {
            await _roleService.AddRole(roleDTO);
            return  Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutRole(UpdateRoleDTO roleDTO)
        {
            await _roleService.UpdateRole(roleDTO);
            return  Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _roleService.DeleteRole(id);
            return  Ok();
        }
    }
}
