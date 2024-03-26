using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.General;
using aspnetcore6.ntier.DAL.Repositories.AccessControl;
using aspnetcore6.ntier.DAL.Repositories.AccessControl.Interfaces;
using aspnetcore6.ntier.DAL.Repositories.Interfaces;

namespace aspnetcore6.ntier.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _context;

        #region General entity registration
        public IRepository<Department> Departments { get; }
        #endregion

        #region Access control entity registration
        public IRepository<User> Users{ get; }
        public IRoleRepository Roles { get; }
        public IRepository<Permission> Permissions { get; }
        #endregion

        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
            Departments = new Repository<Department>(context);
            Users = new Repository<User>(context);
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
