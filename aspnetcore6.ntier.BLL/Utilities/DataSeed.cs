using aspnetcore6.ntier.BLL.Utilities.Interfaces;
using aspnetcore6.ntier.DAL.Models.AccessControl;
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
                #region General entity
                await SeedDepartments();
                #endregion

                #region Access control entity
                await SeedPermissions();
                await SeedRoles();
                await SeedUsers();
                #endregion

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
                await SeedDepartments();
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
                await SeedDepartments();
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
                await SeedDepartments();
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        #endregion

        #region General entitiy seed methods (private)
        private async Task SeedDepartments()
        {
            IEnumerable<Department> departments = await _unitOfWork.Departments.GetAll();
            // Seed only if none exists
            if (!departments.Any())
            {
                string[] departmentNames = {
                    "Administrator",
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
                    await _unitOfWork.Departments.Add(newDepartment);
                }
            }
        }
        #endregion

        #region Access control entity seed methods (private)
        private async Task SeedPermissions()
        {
            IEnumerable<Permission> permissions = await _unitOfWork.Permissions.GetAll();
            // Seed only if none exists
            if (!permissions.Any())
            {
                List<Permission> permissionsToSeed = new List<Permission>()
                {
                    new Permission
                    {
                        Name = "READ",
                        DepartmentId = 1
                    },
                    new Permission
                    {
                        Name = "CREATE",
                        DepartmentId = 1
                    },
                    new Permission
                    {
                        Name = "UPDATE",
                        DepartmentId = 1
                    },
                    new Permission
                    {
                        Name = "DELETE",
                        DepartmentId = 1
                    },
                    new Permission
                    {
                        Name = "READ",
                        DepartmentId = 2
                    },
                    new Permission
                    {
                        Name = "CREATE",
                        DepartmentId = 2
                    },
                    new Permission
                    {
                        Name = "UPDATE",
                        DepartmentId = 2
                    },
                    new Permission
                    {
                        Name = "DELETE",
                        DepartmentId = 2
                    }
                };

                await _unitOfWork.Permissions.AddRange(permissionsToSeed);
            }
        }

        private async Task SeedRoles()
        {
            IEnumerable<Role> roles = await _unitOfWork.Roles.GetAll();
            // Seed only if none exists
            if (!roles.Any())
            {
                List<Role> rolesToSeed = new List<Role>()
                {
                    new Role
                    {
                        Name = "Super Administrator",
                        DepartmentId = 1
                    },
                    new Role
                    {
                        Name = "Administrator",
                        DepartmentId = 2
                    },
                    new Role
                    {
                        Name = "User",
                        DepartmentId = 2
                    },
                    new Role
                    {
                        Name = "Guest",
                        DepartmentId = 2
                    }
                    ,
                    new Role
                    {
                        Name = "Administrator",
                        DepartmentId = 3
                    },
                    new Role
                    {
                        Name = "User",
                        DepartmentId = 3
                    },
                    new Role
                    {
                        Name = "Guest",
                        DepartmentId = 3
                    }
                };

                await _unitOfWork.Roles.AddRange(rolesToSeed);
            }
        }

        private async Task SeedUsers()
        {
            IEnumerable<User> users = await _unitOfWork.Users.GetAll();
            // Seed only if none exists
            if (!users.Any())
            {
                List<User> usersToSeed = new List<User>()
                {
                    new User
                    {
                        UserName = "john_smith",
                        FirstName = "John",
                        LastName = "Smith",
                        Email = "john.smith@example.com",
                        DepartmentId = 1
                    },
                    new User
                    {
                        UserName = "mary_jones",
                        FirstName = "Mary",
                        LastName = "Jones",
                        Email = "mary.jones@example.com",
                        DepartmentId = 2
                    },
                    new User
                    {
                        UserName = "david_wilson",
                        FirstName = "David",
                        LastName = "Wilson",
                        Email = "david.wilson@example.com",
                        DepartmentId = 1
                    },
                    new User
                    {
                        UserName = "laura_davis",
                        FirstName = "Laura",
                        LastName = "Davis",
                        Email = "laura.davis@example.com",
                        DepartmentId = 3
                    },
                    new User
                    {
                        UserName = "mark_thompson",
                        FirstName = "Mark",
                        LastName = "Thompson",
                        Email = "mark.thompson@example.com",
                        DepartmentId = 2
                    },
                    new User
                    {
                        UserName = "susan_miller",
                        FirstName = "Susan",
                        LastName = "Miller",
                        Email = "susan.miller@example.com",
                        DepartmentId = 1
                    },
                    new User
                    {
                        UserName = "chris_roberts",
                        FirstName = "Chris",
                        LastName = "Roberts",
                        Email = "chris.roberts@example.com",
                        DepartmentId = 3
                    },
                    new User
                    {
                        UserName = "emily_walker",
                        FirstName = "Emily",
                        LastName = "Walker",
                        Email = "emily.walker@example.com",
                        DepartmentId = 2
                    },
                    new User
                    {
                        UserName = "james_anderson",
                        FirstName = "James",
                        LastName = "Anderson",
                        Email = "james.anderson@example.com",
                        DepartmentId = 1
                    },
                    new User
                    {
                        UserName = "sarah_harris",
                        FirstName = "Sarah",
                        LastName = "Harris",
                        Email = "sarah.harris@example.com",
                        DepartmentId = 3
                    }
                };

                await _unitOfWork.Users.AddRange(usersToSeed);
            }
        }
        #endregion
    }
}
