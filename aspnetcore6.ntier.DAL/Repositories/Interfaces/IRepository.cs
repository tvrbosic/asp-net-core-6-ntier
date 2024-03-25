using aspnetcore6.ntier.DAL.Models.Abstract;
using System.Linq.Expressions;

namespace aspnetcore6.ntier.DAL.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetAllIncluding(params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetById(int id);
        Task<TEntity> GetByIdIncluding(int id, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        Task Delete(int id);
        void HardDelete(int id);
    }
}