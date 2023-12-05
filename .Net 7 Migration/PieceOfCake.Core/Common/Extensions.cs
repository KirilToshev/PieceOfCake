namespace PieceOfCake.Core.Common;

public static class Extensions
{
    public static bool HasUniqueValuesOnly<T>(this IEnumerable<T> values)
    {
        return values.Distinct().Count() != values.Count();
    }
}
