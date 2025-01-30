namespace LibraryManger.Core
{
    public class Sorting<T>
    {
        public Func<IQueryable<T>, IQueryable<T>>? SortFunction { get; set; }
        public string? Property { get; set; }
        public ESortDirection Direction { get; set; }        
    }

    public enum ESortDirection
    {
        Ascending,
        Descending
    }
}
