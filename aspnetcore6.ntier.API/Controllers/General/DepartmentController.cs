using aspnetcore6.ntier.API.Requests;
using aspnetcore6.ntier.API.Responses;
using aspnetcore6.ntier.Services.DTO.General;
using aspnetcore6.ntier.Services.DTO.Shared;
using aspnetcore6.ntier.Services.Interfaces.General;
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
        public async Task<ActionResult<ApiDataResponse<IEnumerable<DepartmentDTO>>>> GetDepartments()
        {
            IEnumerable<DepartmentDTO> departments = await _departmentService.GetDepartments();
            var response = new ApiDataResponse<IEnumerable<DepartmentDTO>>(departments, "Departments retrieved succcessfully.");
            return Ok(response);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<ApiPagnatedResponse<DepartmentDTO>>> GetPaginatedDepartments([FromQuery] PaginateQueryParameters qp)
        {
            PaginatedDataDTO<DepartmentDTO> pd = await _departmentService.GetPaginatedDepartments(
                qp.PageNumber,
                qp.PageSize,
                qp.searchText,
                qp.orderByProperty,
                qp.ascending);

            var response = new ApiPagnatedResponse<DepartmentDTO>(
                pd.Data,
                pd.PageNumber,
                pd.TotalPages,
                pd.PageSize,
                pd.TotalCount,
                pd.HasPrevious,
                pd.HasNext,
                "Departments retrieved succcessfully.");
            
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiDataResponse<DepartmentDTO>>> GetDepartment(int id)
        {
            DepartmentDTO? department = await _departmentService.GetDepartment(id);
            var response = new ApiDataResponse<DepartmentDTO>(department, "Department retrieved succcessfully.");
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiBaseResponse>> PostDepartment(AddDepartmentDTO departmentDTO)
        {
            await _departmentService.AddDepartment(departmentDTO);
            var response = new ApiBaseResponse("Department creaeted succcessfully.");
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ApiBaseResponse>> PutDepartment(UpdateDepartmentDTO departmentDTO)
        {
            await _departmentService.UpdateDepartment(departmentDTO);
            var response = new ApiBaseResponse("Department updated succcessfully.");
            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ApiBaseResponse>> DeleteDepartment(int id)
        {
            await _departmentService.DeleteDepartment(id);
            var response = new ApiBaseResponse("Department deleted succcessfully.");
            return Ok(response);
        }
    }
}
