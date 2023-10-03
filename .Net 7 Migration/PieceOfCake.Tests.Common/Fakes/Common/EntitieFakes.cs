using PieceOfCake.Core.Common.Entities;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using System.Linq.Expressions;

namespace PieceOfCake.Tests.Common.Fakes.Common;
public abstract class EntitieFakes<TKey, TValue> : BaseFakes
    where TValue : GuidEntity
{
    protected EntitieFakes (IResources resources, IUnitOfWork uowMock) 
        : base(resources, uowMock)
    {
    }

    public abstract Expression<Func<TValue, TKey>> KeyExpression { get; }

    protected TValue GetFromCache (TValue value)
    {
        var key = KeyExpression.Compile().Invoke(value);
        if (Cache.ContainsKey(key))
            return Cache[key];

        Cache.Add(key, value);
        return value;
    }

    private Dictionary<TKey, TValue> Cache { get; set; } = new Dictionary<TKey, TValue>();
}
