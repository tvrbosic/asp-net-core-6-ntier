#nullable enable
using aspnetcore6.ntier.DAL.Exceptions;
using aspnetcore6.ntier.DAL.Interfaces.Repositories;
using aspnetcore6.ntier.DAL.Models.Abstract;
using aspnetcore6.ntier.DAL.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        #region Public repository methods
        // =======================================| IMPORTANT |======================================= //
        /// <summary>
        /// Exposes _dbSet so we could execute additional EF queries on it in case generic repository is already not providing desired functionality.
        /// This approach removes possible situation in which we would create custom repository for specific case and attach it to UnitOfWork.
        /// </summary>
        /// <returns>Queryable DbSet of type TEntity.</returns>
        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<PaginatedData<TEntity>> GetAllPaginated(
            int PageNumber, 
            int PageSize, 
            string? searchInput, 
            string[]? 
            searchProperties, 
            string orderByProperty  = "Id", 
            bool ascending = true)
        {
            var filteredEntities = SearchFilter(_dbSet.AsNoTracking(), searchInput, searchProperties);
            filteredEntities = OrderByProperty(filteredEntities, orderByProperty, ascending);

            return await PaginatedData<TEntity>.ToPaginatedData(filteredEntities, PageNumber, PageSize);
        }

        public async Task<IEnumerable<TEntity>> GetAllIncluding(params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = _dbSet.AsNoTracking().AsQueryable();
            
            foreach (var includeProperty in includes)
            {
                entities = entities.Include(includeProperty);
            }

            return await entities.AsNoTracking().ToListAsync();

        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = _dbSet.AsQueryable();

            foreach (var include in includes)
            {
                entities = entities.Include(include);
            }

            return await entities.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity> GetByIdIncluding(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = _dbSet.AsQueryable();

            foreach (var include in includes)
            {
                entities = entities.Include(include);
            }

            return await entities.FirstOrDefaultAsync(e => e.Id == id);
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
                throw new EntityNotFoundException($"Update operation failed for entitiy {typeof(TEntity)} with id: {entity.Id}");
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
        private static IQueryable<TEntity> SearchFilter(IQueryable<TEntity> entities, string? searchInput, string[]? searchProperties)
        {
            IQueryable<TEntity> filteredEntities = entities;

            if (!string.IsNullOrEmpty(searchInput) && searchProperties != null && searchProperties.Any())
            {
                foreach (var sp in searchProperties)
                {
                    // For each provided searchProperty check if it exists on entity type
                    var targetProperty = entities.ElementType.GetProperty(sp);
                    if (targetProperty != null)
                    {
                        // Filter entities whose searchProperty contains searchInput
                        filteredEntities = entities.Where(e => EF.Property<string>(e, sp).Contains(searchInput));
                    }
                }
            }

            return filteredEntities;
        }

        public IQueryable<TEntity> OrderByProperty(IQueryable<TEntity> entities, string orderByProperty = "Id", bool ascending = true)
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
            var labmdaProperty = Expression.Property(lambdaParameter, orderByProperty);
            // Lambda expression for the sorting condition (x => x.orderByProperty)
            var lambda = Expression.Lambda<Func<TEntity, int>>(labmdaProperty, lambdaParameter);

            // Return ordered 
            return ascending ? entities.OrderBy(lambda) : entities.OrderByDescending(lambda);
            #endregion
        }
    }
}
