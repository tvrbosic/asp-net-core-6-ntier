using aspnetcore6.ntier.BLL.DTOs.General;
using aspnetcore6.ntier.BLL.DTOs.Shared;
using aspnetcore6.ntier.BLL.Interfaces.General;
using aspnetcore6.ntier.DAL.Interfaces.Repositories;
using aspnetcore6.ntier.DAL.Models.General;
using aspnetcore6.ntier.DAL.Models.Shared;
using AutoMapper;

namespace aspnetcore6.ntier.BLL.Services.General
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DepartmentDTO>> GetDepartments()
        {
            IEnumerable<Department> departments = await _unitOfWork.Departments.GetAll();
            IEnumerable<DepartmentDTO> departmentDTOs = _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
            return departmentDTOs;
        }

        public PaginatedDataDTO<DepartmentDTO> GetPaginatedDepartments(
            int PageNumber,
            int PageSize,
            string? searchText,
            string orderByProperty = "Id",
            bool ascending = true)
        {
            Func<Department, bool>? searchTextPredicate = null;
            if (!string.IsNullOrEmpty(searchText))
            {
                searchTextPredicate = p => p.Name.Contains(searchText);
            }

            PaginatedData<Department> paginatedDepartments = _unitOfWork.Departments.GetAllPaginated(
                PageNumber,
                PageSize,
                searchTextPredicate,
                orderByProperty,
                ascending);
            PaginatedDataDTO<DepartmentDTO> paginatedDepartmentDTOs = _mapper.Map<PaginatedDataDTO<DepartmentDTO>>(paginatedDepartments);

            return paginatedDepartmentDTOs;
        }

        public async Task<DepartmentDTO?> GetDepartment(int id)
        {
            Department? department = await _unitOfWork.Departments.GetById(id);
            DepartmentDTO departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return departmentDTO;
        }

        public async Task AddDepartment(AddDepartmentDTO departmentDTO)
        {
            Department department = _mapper.Map<Department>(departmentDTO);
             await _unitOfWork.Departments.Add(department);
             await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateDepartment(UpdateDepartmentDTO departmentDTO)
        {
            Department department = _mapper.Map<Department>(departmentDTO);
            await _unitOfWork.Departments.Update(department);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteDepartment(int id)
        {
            await _unitOfWork.Departments.Delete(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
