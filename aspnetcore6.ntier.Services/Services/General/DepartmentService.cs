using aspnetcore6.ntier.Services.DTO.General;
using aspnetcore6.ntier.Services.DTO.Shared;
using aspnetcore6.ntier.Services.Interfaces.General;
using aspnetcore6.ntier.DataAccess.Interfaces.Repositories;
using aspnetcore6.ntier.Models.General;
using aspnetcore6.ntier.Models.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace aspnetcore6.ntier.Services.Services.General
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

        public async Task<PaginatedDataDTO<DepartmentDTO>> GetPaginatedDepartments(
            int PageNumber,
            int PageSize,
            string? searchText,
            string orderByProperty = "Id",
            bool ascending = true)
        {
            Expression<Func<Department, bool>>? searchTextPredicate = null;
            if (!string.IsNullOrEmpty(searchText))
            {
                searchTextPredicate = p => p.Name.ToLower().Contains(searchText.ToLower());
            }

            PaginatedData<Department> paginatedDepartments = await _unitOfWork.Departments.GetAllPaginated(
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
