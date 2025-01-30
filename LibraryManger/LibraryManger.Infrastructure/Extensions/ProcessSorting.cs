using LibraryManger.Core;

namespace LibraryManger.Infrastructure.Extensions
{
    public class ProcessSorting<T>
    {
        public virtual IQueryable<T> Apply(Sorting<T> sorting, IQueryable<T> query)
        {
            if (sorting.SortFunction != null)
                return sorting.SortFunction(query);

            if (string.IsNullOrEmpty(sorting.Property?.Trim()))
                return query;

            return sorting.Direction == ESortDirection.Ascending ? query.OrderBy(sorting.Property) : query.OrderByDescending(sorting.Property);
        }
    }
}
