using aspnetcore6.ntier.BLL.Services.AccessControl.DTOs;
using aspnetcore6.ntier.BLL.Services.AccessControl.Interfaces;
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
            return Ok(permissions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionDTO>> GetPermission(int id)
        {
            PermissionDTO permission = await _permissionService.GetPermission(id);
            return Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult> PostPermission(AddPermissionDTO permissionDTO)
        {
            await _permissionService.AddPermission(permissionDTO);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutPermission(UpdatePermissionDTO permissionDTO)
        {
            await _permissionService.UpdatePermission(permissionDTO);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePermission(int id)
        {
            await _permissionService.DeletePermission(id);
            return Ok();
        }
    }
}
