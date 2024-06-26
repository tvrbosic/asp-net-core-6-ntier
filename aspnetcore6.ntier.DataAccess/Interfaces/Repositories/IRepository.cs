﻿using aspnetcore6.ntier.Models.Abstract;
using aspnetcore6.ntier.Models.Shared;
using System.Linq.Expressions;

namespace aspnetcore6.ntier.DataAccess.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<PaginatedData<TEntity>> GetAllPaginated(
            int PageNumber, 
            int PageSize, 
            Expression<Func<TEntity, bool>>? searchTextPredicate, 
            string orderByProperty  = "Id", 
            bool ascending = true);
        Task<IEnumerable<TEntity>> GetAllIncluding(params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetById(int id);
        Task<TEntity> GetByIdIncluding(int id, params Expression<Func<TEntity, object>>[] includes);
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        Task Update(TEntity entity);
        Task Delete(int id);
    }
}