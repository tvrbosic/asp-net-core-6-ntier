using aspnetcore6.ntier.DataAccess.Interfaces.Repositories;
using aspnetcore6.ntier.DataAccess.Repositories.AccessControl;
using aspnetcore6.ntier.Models.AccessControl;
using aspnetcore6.ntier.Models.General;

namespace aspnetcore6.ntier.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _context;

        #region General entity registration
        public IRepository<Department> Departments { get; }
        #endregion

        #region Access control entity registration
        public IRepository<Permission> Permissions { get; }
        public RoleRepository Roles { get; }
        public UserRepository Users { get; }
        #endregion

        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
            Departments = new Repository<Department>(context);
            Permissions = new Repository<Permission>(context);
            Roles = new RoleRepository(context);
            Users = new UserRepository(context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
