using aspnetcore6.ntier.DAL.Interfaces.Repositories;
using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Models.General;

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
        public IRepository<Role> Roles { get; }
        public IRepository<Permission> Permissions { get; }
        #endregion

        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
            Departments = new Repository<Department>(context);
            Users = new Repository<User>(context);
            Roles = new Repository<Role>(context);
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
