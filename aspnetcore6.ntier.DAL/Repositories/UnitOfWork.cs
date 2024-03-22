using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Repositories.Interfaces;

namespace aspnetcore6.ntier.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _context;

        public IRepository<Department> Departments { get; }

        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
            Departments = new Repository<Department>(context);
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
