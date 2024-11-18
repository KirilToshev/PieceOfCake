using PieceOfCake.Core.Common.Entities;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using System.Linq.Expressions;

namespace PieceOfCake.Tests.Common.Fakes.Common;
public abstract class EntityFakes<TKey, TValue> : BaseFakes
    where TValue : GuidEntity
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();

    protected EntityFakes (IResources resources, IUnitOfWork uowMock) 
        : base(resources, uowMock)
    {
    }

    public abstract Expression<Func<TValue, TKey>> CacheKey { get; }

    protected TValue GetFromCache (TValue value)
    {
        var key = CacheKey.Compile().Invoke(value);
        if (_cache.ContainsKey(key))
            return _cache[key];

        _cache.Add(key, value);
        return value;
    }
}
