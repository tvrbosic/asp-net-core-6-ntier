﻿using aspnetcore6.ntier.API.Requests;
using aspnetcore6.ntier.API.Responses;
using aspnetcore6.ntier.BLL.DTOs.General;
using aspnetcore6.ntier.BLL.DTOs.Shared;
using aspnetcore6.ntier.BLL.Interfaces.General;
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
            var response = new ApiDataResponse<IEnumerable<DepartmentDTO>>(departments, "Departments retrieved succcessfully.");
            return Ok(response);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetPaginatedDepartments([FromQuery] PaginateQueryParameters queryParameters)
        {
            PaginatedDataDTO<DepartmentDTO> paginatedDepartments = await _departmentService.GetPaginatedDepartments(queryParameters.CurrentPage, queryParameters.PageSize);
            var response = new ApiPagnatedResponse<DepartmentDTO>(
                paginatedDepartments.Data,
                paginatedDepartments.CurrentPage,
                paginatedDepartments.TotalPages,
                paginatedDepartments.PageSize,
                paginatedDepartments.TotalCount,
                paginatedDepartments.HasPrevious,
                paginatedDepartments.HasNext,
                "Departments retrieved succcessfully.");
            
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartment(int id)
        {
            DepartmentDTO department = await _departmentService.GetDepartment(id);
            var response = new ApiDataResponse<DepartmentDTO>(department, "Department retrieved succcessfully.");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostDepartment(AddDepartmentDTO departmentDTO)
        {
            await _departmentService.AddDepartment(departmentDTO);
            var response = new ApiBaseResponse("Department creaeted succcessfully.");
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> PutDepartment(UpdateDepartmentDTO departmentDTO)
        {
            await _departmentService.UpdateDepartment(departmentDTO);
            var response = new ApiBaseResponse("Department updated succcessfully.");
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            await _departmentService.DeleteDepartment(id);
            var response = new ApiBaseResponse("Department deleted succcessfully.");
            return Ok(response);
        }
    }
}
