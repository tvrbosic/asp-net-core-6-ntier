using aspnetcore6.ntier.DataAccess.Exceptions;
using aspnetcore6.ntier.DataAccess.Interfaces.Repositories;
using aspnetcore6.ntier.Models.Abstract;
using aspnetcore6.ntier.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace aspnetcore6.ntier.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApiDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(ApiDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        #region Public repository methods
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<PaginatedData<TEntity>> GetAllPaginated(
            int PageNumber,
            int PageSize,
            Expression<Func<TEntity, bool>>? searchTextPredicate,
            string orderByProperty = "Id",
            bool ascending = true)
        {
            // Search
            var filteredEntities = _dbSet.AsNoTracking();
            if (searchTextPredicate != null)
            {
                filteredEntities = _dbSet.Where(searchTextPredicate).AsQueryable();
            }

            // Order
            filteredEntities = OrderByProperty(filteredEntities, orderByProperty, ascending);

            // Paginate
            return await PaginateData(filteredEntities, PageNumber, PageSize);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllIncluding(params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = _dbSet.AsNoTracking().AsQueryable();

            foreach (var includeProperty in includes)
            {
                entities = entities.Include(includeProperty);
            }

            return await entities.AsNoTracking().ToListAsync();

        }

        public virtual async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> result = await _dbSet.Where(predicate).ToListAsync();
            if (result == null)
            {
                throw new EntityNotFoundException($"Find operation failed for entitiy {typeof(TEntity)}");
            }

            return result;
        }

        public virtual async Task<IEnumerable<TEntity>> FindIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = _dbSet.AsQueryable();

            foreach (var include in includes)
            {
                entities = entities.Include(include);
            }

            IEnumerable<TEntity> result = await entities.Where(predicate).ToListAsync();

            if (result == null)
            {
                throw new EntityNotFoundException($"Find operation failed for entitiy {typeof(TEntity)}");
            }

            return result;
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            TEntity? result = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

            if (result == null)
            {
                throw new EntityNotFoundException($"Get operation failed for entitiy {typeof(TEntity)} with id: {id}");
            }

            return result;
        }

        public virtual async Task<TEntity> GetByIdIncluding(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = _dbSet.AsQueryable();

            foreach (var include in includes)
            {
                entities = entities.Include(include);
            }

            TEntity? result = await entities.FirstOrDefaultAsync(e => e.Id == id);

            if (result == null)
            {
                throw new EntityNotFoundException($"Get operation failed for entitiy {typeof(TEntity)} with id: {id}");
            }

            return result;
        }

        public virtual async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual async Task Update(TEntity entity)
        {
            var existingEntity = await GetById(entity.Id);

            if (existingEntity != null)
            {
                // Check if the entity is already being tracked
                var entry = _context.Entry(existingEntity);
                if (entry.State == EntityState.Detached)
                {
                    // If it's detached, attach the provided entity
                    _dbSet.Attach(entity);
                }
                else
                {
                    // If it's already being tracked, update its properties with the provided entity
                    entry.CurrentValues.SetValues(entity);
                }

                // Set the state of the entity to Modified
                entry.State = EntityState.Modified;
            }
            else
            {
                throw new EntityNotFoundException($"Update operation failed for entity {typeof(TEntity)} with id: {entity.Id}");
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
                throw new EntityNotFoundException($"Delete operation failed for entity {typeof(TEntity)} with id: {id}");
            }
        }
        #endregion

        #region Private repository methods
        private IQueryable<TEntity> OrderByProperty(IQueryable<TEntity> entities, string orderByProperty = "Id", bool ascending = true)
        {
            // Check if the orderByProperty exists in the TEntity type
            var entityType = typeof(TEntity);
            var entityProperty = entityType.GetProperty(orderByProperty);

            if (entityProperty == null)
            {
                throw new ArgumentException($"The property '{orderByProperty}' does not exist in the entity type '{entityType.Name}'.");
            }

            // Parameter expression for the input parameter to the lambda expression
            var lambdaParameter = Expression.Parameter(typeof(TEntity), "e");
            // Member access expression for accessing the property to be sorted
            var lambdaProperty = Expression.Property(lambdaParameter, orderByProperty);

            // Lambda expression for the sorting condition (x => x.orderByProperty). Convert is necessary because orderByProperty can be of different types.
            var lambda = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(lambdaProperty, typeof(object)), lambdaParameter);

            // Return ordered 
            return ascending ? entities.OrderBy(lambda) : entities.OrderByDescending(lambda);
        }

        private async Task<PaginatedData<TEntity>> PaginateData(IQueryable<TEntity> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var entities = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedData<TEntity>(entities, count, pageNumber, pageSize);
        }
        #endregion
    }
}
