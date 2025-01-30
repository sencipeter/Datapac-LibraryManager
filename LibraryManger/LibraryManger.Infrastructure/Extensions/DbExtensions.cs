using LibraryManger.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace LibraryManger.Infrastructure.Extensions
{
    public static class DbExtensions
    {
        public static ListResult<T> PaginateAndExecute<T>(this IQueryable<T> query, Pagination pagination)
        {
            return new ListResult<T>
            {
                TotalCount = query.Count(),
                Result = pagination == null ? query.ToList() : pagination.Apply(query).ToList()
            };
        }

        public static async Task<ListResult<T>> PaginateAndExecuteAsync<T>(this IQueryable<T> query, Pagination? pagination)
        {
            return new ListResult<T>
            {
                CurrentPage = pagination?.CurrentPage ?? 1,
                PageSize= pagination?.PageSize ?? 0,
                TotalCount = query.Count(),
                Result = pagination == null ? await query.ToListAsync() : await pagination.Apply(query).ToListAsync()
            };
        }

        public static ListResult<TOutput> PaginateExecuteAndCast<TOutput, TInput>(this IQueryable<TInput> query, Pagination pagination)
        {
            var result = PaginateAndExecute(query, pagination);
            return new ListResult<TOutput>
            {
                Result = result.Result.Cast<TOutput>().ToList(),
                TotalCount = result.TotalCount
            };
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string fullPropertyName)
        {
            var propertyNameParts = fullPropertyName.Split('.');
            var propertyName = propertyNameParts[0];

            var entityType = typeof(TSource);
            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     //Put more restriction here to ensure selecting the right overload                
                     return parameters.Count == 2;//overload that has 2 parameters
                 }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method
                 .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            var newQuery = (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> query, string fullPropertyName)
        {
            var propertyNameParts = fullPropertyName.Split('.');
            var propertyName = propertyNameParts[0];
            var entityType = typeof(TSource);

            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     //Put more restriction here to ensure selecting the right overload                
                     return parameters.Count == 2;//overload that has 2 parameters
                 }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method
                 .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            var newQuery = (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        public static IOrderedQueryable<TSource> ApplySortDirection<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, ESortDirection sortDirection)
        {
            var orderedSource = source as IOrderedQueryable<TSource>;
            if (orderedSource != null)
            {
                switch (sortDirection)
                {
                    case ESortDirection.Descending:
                        return (orderedSource.ThenByDescending(keySelector));

                    default:
                        return (orderedSource.ThenBy(keySelector));
                }
            }
            else
            {
                switch (sortDirection)
                {
                    case ESortDirection.Descending:
                        return (source.OrderByDescending(keySelector));

                    default:
                        return (source.OrderBy(keySelector));
                }
            }
        }
    }
}
