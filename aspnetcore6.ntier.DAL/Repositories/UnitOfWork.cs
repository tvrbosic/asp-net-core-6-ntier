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
        public IRepository<ApplicationUser> Users{ get; }
        public RoleRepository Roles { get; }
        public IRepository<Permission> Permissions { get; }
        #endregion

        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
            Departments = new Repository<Department>(context);
            Users = new Repository<ApplicationUser>(context);
            Roles = new RoleRepository(context);
            Permissions = new Repository<Permission>(context);
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
