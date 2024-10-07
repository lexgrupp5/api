namespace Shared.Extensions;

public static class CollectionExtensions
{
    public static void AddNotNull<T>(this ICollection<T> collection, T? item)
    {
        if (item != null)
            collection.Add(item);
    }
}
