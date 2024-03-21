using aspnetcore6.ntier.DAL.Models.Abstract;
using System.Linq.Expressions;

namespace aspnetcore6.ntier.DAL.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(int id);
        void HardDelete(int id);
    }
}