namespace Pronia.ViewModels
{
    public class CustomPaginatedList<T>
    {
        public CustomPaginatedList(List<T> items, int pageIndex, int totalPage)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalPage = totalPage;
        }
        public bool HasNext => PageIndex < TotalPage;
        public bool HasPrev => PageIndex > 1;
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }

        public static CustomPaginatedList<T> CreateCustomList(IQueryable<T> query, int page, int size)
        {
            int total = (int)Math.Ceiling(query.Count() / (double)size);
            return new CustomPaginatedList<T>(query.Skip((page - 1) * size).Take(size).ToList(), page, total);
        }
    }
}


/////////////////////////
///
