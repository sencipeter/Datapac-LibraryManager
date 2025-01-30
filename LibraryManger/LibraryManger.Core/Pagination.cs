namespace LibraryManger.Core
{
    public class Pagination
    {
        public int PageOffset
        {
            get
            {
                return (CurrentPage - 1) * PageSize;
            }
        }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public IQueryable<T> Apply<T>(IQueryable<T> query)
        {
            if (CurrentPage < 1)
                CurrentPage = 1;

            return query.Skip(PageOffset).Take(PageSize);
        }
    }
}
