using aspnetcore6.ntier.DAL.Models.AccessControl;
using aspnetcore6.ntier.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore6.ntier.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public IRepository<Department> DepartmentsRepository { get; }

        public UnitOfWork(DbContext context)
        {
            _context = context;
            DepartmentsRepository = new Repository<Department>(context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}
