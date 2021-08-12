namespace Sample.Rest.Api.Extensions
{
    public abstract class Parameter
    {
        // Constants

        private const int MaxPageSize = 25;

        // Fields

        private int _pageSize = 10;
        private string _orderBy;
        private string _query;

        // Properties

        public int Page { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string Query
        {
            get => _query;
            set => _query = !string.IsNullOrWhiteSpace(value) ? value.Trim().ToLowerInvariant() : null;
        }

        internal bool HasQuery => !string.IsNullOrWhiteSpace(_query);

        public string OrderBy
        {
            get => _orderBy;
            set => _orderBy = !string.IsNullOrWhiteSpace(value) ? value.Trim().ToLowerInvariant() : null;
        }

        internal bool IgnoreItems { get; set; }
    }
}