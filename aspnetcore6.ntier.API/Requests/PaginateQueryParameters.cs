namespace aspnetcore6.ntier.API.Requests
{
    public class PaginateQueryParameters
    {
        const int maxPageSize = 100;
        public int CurrentPage { get; set; } = 1;
        private int _pageSize = 10;
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
    }
}
