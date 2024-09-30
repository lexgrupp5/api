using System.Linq.Expressions;

namespace Shared.Extensions;

public static class QueryExtensions
{
    public static bool IsOrdered<T>(this IQueryable<T> queryable) =>
        queryable == null
            ? throw new ArgumentNullException(nameof(queryable))
            : queryable.Expression.Type.IsGenericType
                && queryable.Expression.Type.GetGenericTypeDefinition()
                    == typeof(IOrderedQueryable<>);

    public static IQueryable<T> SmartOrderBy<T, TKey>(
        this IQueryable<T> queryable,
        Expression<Func<T, TKey>> keySelector,
        bool descending = false
    ) =>
        queryable.IsOrdered()
            ? descending
                ? ((IOrderedQueryable<T>)queryable).ThenByDescending(keySelector)
                : ((IOrderedQueryable<T>)queryable).ThenBy(keySelector)
            : (IQueryable<T>)(
                descending
                    ? queryable.OrderByDescending(keySelector)
                    : queryable.OrderBy(keySelector)
            );
}
