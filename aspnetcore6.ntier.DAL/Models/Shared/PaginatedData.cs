using aspnetcore6.ntier.DAL.Models.Abstract;

namespace aspnetcore6.ntier.DAL.Models.Shared
{
    public class PaginatedData<TEntity> where TEntity : BaseEntity
    {
        public PaginatedData(IEnumerable<TEntity> entities, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Data = entities;
        }

        public IEnumerable<TEntity> Data { get; private set; }
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
        

        public static PaginatedData<TEntity> ToPaginatedData(IQueryable<TEntity> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var entities = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedData<TEntity>(entities, count, pageNumber, pageSize);
        }
    }
}
