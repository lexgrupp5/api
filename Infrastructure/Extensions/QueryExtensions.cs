using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.Models;

namespace Infrastructure.Extensions;

public static class QueryExtensions
{
    public static IQueryable<T> ApplyFilters<T>(
        this IQueryable<T> query,
        IEnumerable<Expression<Func<T, bool>>>? filters
    )
    {
        return filters?.Aggregate(query, (cur, filter) => cur.Where(filter)) ?? query;
    }

    /*
     *
     ****/
    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> query,
        IEnumerable<SortParams>? sorting
    )
    {
        if (sorting is null || !sorting.Any())
            return query;

        foreach (var sort in sorting)
        {
            var propInfo = typeof(T).GetProperty(
                sort.Field,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
            );
            if (propInfo == null)
                continue;

            var parameter = Expression.Parameter(typeof(T), propInfo.Name);
            var propertyAccess = Expression.MakeMemberAccess(parameter, propInfo);
            var lambda =
                (Expression<Func<T, object>>)
                    Expression.Lambda(
                        typeof(Func<,>).MakeGenericType(typeof(T), typeof(object)),
                        propertyAccess,
                        parameter
                    );
            query = query.SmartOrderBy(lambda, sort.Descending);
        }

        return query;
    }

    /*
     *
     ****/
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, PageParams? pagination)
    {
        if (pagination == null)
            return query;

        pagination.Page = Math.Max(pagination.Page, 1);
        pagination.Size = pagination.Size > 0 ? pagination.Size : 10;

        return query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size);
    }
}
