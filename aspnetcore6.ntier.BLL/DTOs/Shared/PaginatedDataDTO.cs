namespace aspnetcore6.ntier.BLL.DTOs.Shared
{
    public class PaginatedDataDTO<TEntity>
    {
        public IEnumerable<TEntity> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
}