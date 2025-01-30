namespace LibraryManger.Core
{
    public class ListResult<T>
    {
        public ListResult()
        {
            Result = new List<T>();
        }

        public IList<T> Result { get; set; }
        public Exception? Exception { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage
        {
            get
            {
                if (PageSize == 0)
                    return 0;

                var totalPage = TotalCount / PageSize;
                if (TotalCount % PageSize > 0)
                    totalPage = totalPage + 1;
                return totalPage;
            }
        }

        public IList<T> GetResultOrThrowException()
        {
            if (Exception != null)
                throw Exception;
            return Result;
        }
    }
}
