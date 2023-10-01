using AutoFixture;

namespace PieceOfCake.Tests.Common;

public static class Extensions
{
    public static IEnumerable<T> RandomListOf<T>(this Fixture fixture, params T[]  objects)
    {
        var random = new Random();
        var distinct = objects.Distinct().ToArray();
        var randomLength = random.Next(1, distinct.Length);
        if (randomLength == distinct.Length)
            return distinct;

        var list = new HashSet<T>();
        while (list.Count() < randomLength)
        {
            var randomIndex = random.Next(0, distinct.Length - 1);
            var item = distinct[randomIndex];
            if (!list.Contains(item)) 
            {
                list.Add(item);
            }
        }
        return list;
    }

    public static T OneOf<T> (this Fixture fixture, params T[] objects)
    {
        var random = new Random();
        var distinct = objects.Distinct().ToArray();
        var randomIndex = random.Next(1, distinct.Length - 1);
        return objects[randomIndex];
    }
}
