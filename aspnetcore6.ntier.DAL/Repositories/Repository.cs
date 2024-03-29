using aspnetcore6.ntier.DAL.Exceptions;
using aspnetcore6.ntier.DAL.Interfaces.Repositories;
using aspnetcore6.ntier.DAL.Models.Abstract;
using aspnetcore6.ntier.DAL.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace aspnetcore6.ntier.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApiDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ApiDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<PaginatedData<TEntity>> GetAllPaginated(int PageNumber, int PageSize)
        {
            var entries = _dbSet.AsNoTracking();
            return await PaginatedData<TEntity>.ToPaginatedData(entries, PageNumber, PageSize);
        }

        public async Task<IEnumerable<TEntity>> GetAllIncluding(params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();
            foreach (var includeProperty in includes)
            {
                query = query.Include(includeProperty);
            }
            return await query.AsNoTracking().ToListAsync();

        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity> GetByIdIncluding(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual async Task Update(TEntity entity)
        {
            var existingEntity = await GetById(entity.Id);
            if (existingEntity != null)
            {
                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                throw new EntityNotFoundException("$Update operation failed for entitiy {entity.GetType()} with id: {id}");
            }
        }

        public virtual async Task Delete(int id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _context.Remove(entity);
            }
            else
            {
                throw new EntityNotFoundException("$Delete operation failed for entitiy {entity.GetType()} with id: {id}");
            }
        }
    }
}
