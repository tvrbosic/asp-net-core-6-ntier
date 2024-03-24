using aspnetcore6.ntier.BLL.Utilities.Interfaces;
using aspnetcore6.ntier.DAL.Models.General;
using aspnetcore6.ntier.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace aspnetcore6.ntier.BLL.Utilities
{
    public class DataSeed : IDataSeed
    {
        private readonly ILogger<DataSeed> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DataSeed(ILogger<DataSeed> logger, IUnitOfWork unitOfWOrk)
        {
            _logger = logger;
            _unitOfWork = unitOfWOrk;
        }

        #region Environment seed methods (public)
        public async Task DevelopmentDataSeed()
        {
            try
            {
                SeedDepartments();
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task TestDataSeed()
        {
            try
            {
                SeedDepartments();
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task UatDataSeed()
        {
            try
            {
                SeedDepartments();
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task ProductionDataSeed()
        {
            try
            {
                SeedDepartments();
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        #endregion

        #region General entitiy seed methods (private)
        private void SeedDepartments()
        {
            // Seed only if none exists
            if (!_unitOfWork.Departments.GetAll().Any())
            {
                string[] departmentNames = {
                    "Department of Technology",
                    "Sales Department",
                    "Marketing Department",
                    "Human Resources Department",
                    "Finance Department",
                    "Customer Service Department",
                    "Research and Development Department",
                    "Production Department",
                    "Quality Assurance Department",
                    "Legal Department"
                };

                foreach (string name in departmentNames)
                {
                    var newDepartment = new Department { Name = name };
                    _unitOfWork.Departments.Add(newDepartment);
                }
            }
        }
        #endregion

        #region Access control entity seed methods (private)
        private void SeedPermissions()
        {
            // Seed only if none exists
            if (!_unitOfWork.Permissions.GetAll().Any())
            {
                // TODO
            }
        }

        private void SeedRoles()
        {
            // Seed only if none exists
            if (!_unitOfWork.Roles.GetAll().Any())
            {
                // TODO
            }
        }

        private void SeedUsers()
        {
            // Seed only if none exists
            if (!_unitOfWork.Users.GetAll().Any())
            {
                // TODO
            }
        }
        #endregion
    }
}
