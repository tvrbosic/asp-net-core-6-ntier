﻿using aspnetcore6.ntier.BLL.Interfaces.Utilities;
using aspnetcore6.ntier.DAL.Interfaces.Repositories;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.General;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace aspnetcore6.ntier.BLL.Utilities
{
    public class DataSeed : IDataSeed
    {
        private readonly ILogger<DataSeed> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApiDbContext _context;

        public DataSeed(ILogger<DataSeed> logger, IUnitOfWork unitOfWOrk, ApiDbContext context)
        {
            _logger = logger;
            _unitOfWork = unitOfWOrk;
            _context = context;
        }

        #region Environment seed methods (public)
        public async Task DevelopmentDataSeed()
        {
            try
            {
                #region Seed superuser
                await SeedSuperuser();
                #endregion

                #region General entity
                await SeedDepartments();
                #endregion

                #region Access control entity
                await SeedPermissions();
                await SeedRoles();
                await SeedUsers();
                #endregion

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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        #endregion

        #region Seed superuser
        private async Task SeedSuperuser()
        {
            IEnumerable<User> users = await _unitOfWork.Users.GetAll();

            // Seed only if none exists
            if (!users.Any())
            { 
                try
                {
                    _context.Database.ExecuteSqlRaw(@"INSERT INTO Users (UserName, FirstName, LastName, Email, DateCreated, IsDeleted) 
                                                        VALUES (@UserName, @FirstName, @LastName, @Email, @DateCreated, @IsDeleted)",
                        new[]
                        {
                            new SqlParameter("@UserName", "SUPERUSER"),
                            new SqlParameter("@FirstName", "SUPER"),
                            new SqlParameter("@LastName", "USER"),
                            new SqlParameter("@Email", "super.user@email.com"),
                            new SqlParameter("@DateCreated", DateTime.UtcNow),
                            new SqlParameter("@IsDeleted", false)
                    });
                    _logger.LogInformation("Super user account seeded successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while seeding super user.");
                }
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

                await _unitOfWork.CompleteAsync();
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
                await _unitOfWork.CompleteAsync();
            }
        }
        
        private async Task SeedRoles()
        {
            IEnumerable<Role> roles = await _unitOfWork.Roles.GetAll();

            // Seed only if none exists
            if (!roles.Any())
            {
                IEnumerable<Department> departments = await _unitOfWork.Departments.GetAll();
                Random random = new Random();

                string[] roleNames = {
                    "Super Administrator",
                    "Administrator",
                    "User",
                    "Guest",
                    "Administrator",
                    "User",
                    "Guest",
                };

                foreach (string roleName in roleNames)
                {
                    Role addRole = new Role()
                    {
                        Name = roleName,
                        DepartmentId = random.Next(1, departments.Count() + 1)
                    };
                
                    for (int i = 1; i < 4; i++)
                    {
                        Permission permissionToAdd = await _unitOfWork.Permissions.GetById(i);
                        addRole.PermissionsLink.Add(new PermissionRoleLink
                        {
                            Role = addRole,
                            Permission = permissionToAdd
                        });
                    }

                    await _unitOfWork.Roles.Add(addRole);
                }
                await _unitOfWork.CompleteAsync();
            }
        }

        private async Task SeedUsers()
        {
            IEnumerable<User> users = await _unitOfWork.Users.GetAll();
            // Seed users only if there is just one user (Superuser)
            if (users.Any() && !(users.Count() == 1))
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
                await _unitOfWork.CompleteAsync();
            }
        }
        #endregion
    }
}
