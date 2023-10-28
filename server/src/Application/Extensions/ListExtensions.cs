namespace StudyZen.Application.Extensions;

public static class ListExtensions
{
    public static IEnumerable<IEnumerable<T>> Split<T>(this IReadOnlyCollection<T> source, int nParts)
    {
        return source
            .Select((item, index) => new { item, index })
            .GroupBy(x => x.index % nParts)
            .Select(x => x.Select(y => y.item));
    }
}