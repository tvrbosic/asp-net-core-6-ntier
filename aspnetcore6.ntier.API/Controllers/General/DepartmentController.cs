using aspnetcore6.ntier.API.Responses;
using aspnetcore6.ntier.BLL.Services.General.DTOs;
using aspnetcore6.ntier.BLL.Services.General.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore6.ntier.API.Controllers.General
{
    [ApiController]
    [Route("api/department")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartments()
        {
            IEnumerable<DepartmentDTO> departments = await _departmentService.GetDepartments();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartment(int id)
        {
            DepartmentDTO department = await _departmentService.GetDepartment(id);
            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> PostDepartment(AddDepartmentDTO departmentDTO)
        {
            await _departmentService.AddDepartment(departmentDTO);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutDepartment(UpdateDepartmentDTO departmentDTO)
        {
            await _departmentService.UpdateDepartment(departmentDTO);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            await _departmentService.DeleteDepartment(id);
            return Ok(); ;
        }
    }
}
