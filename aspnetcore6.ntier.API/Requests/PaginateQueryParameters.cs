namespace aspnetcore6.ntier.API.Requests
{
    public class PaginateQueryParameters
    {
        
        const int maxPageSize = 100; // If provided page size is greater this constant will be used
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public string? searchText { get; set; } = null;
        public string orderByProperty { get; set; } = "Id";
        public bool ascending { get; set; } = true;
    }
}
