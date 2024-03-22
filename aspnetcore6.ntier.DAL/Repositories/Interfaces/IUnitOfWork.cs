using aspnetcore6.ntier.DAL.Models.AccessControl;

namespace aspnetcore6.ntier.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Department> Departments { get; }

        Task<int> CompleteAsync();
    }
}
