using aspnetcore6.ntier.BLL.Services.General.DTOs;
using aspnetcore6.ntier.BLL.Services.General.Interfaces;
using aspnetcore6.ntier.DAL.Models.General;
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
            return departments == null ? NotFound() : Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartment(int id)
        {
            DepartmentDTO department = await _departmentService.GetDepartment(id);
            return department == null ? NotFound() : Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> PostDepartment(AddDepartmentDTO departmentDTO)
        {
            return await _departmentService.AddDepartment(departmentDTO) ? Ok() : BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> PutDepartment(UpdateDepartmentDTO departmentDTO)
        {
            return await _departmentService.UpdateDepartment(departmentDTO) ? Ok() : BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            return await _departmentService.DeleteDepartment(id) ? Ok() : BadRequest();
        }
    }
}
