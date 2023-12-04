namespace PieceOfCake.Core.Common.ValueObjects;

public abstract class ValueObject<T, K> : CSharpFunctionalExtensions.ValueObject<T>
    where T : ValueObject<T, K>
    where K : notnull
{
    protected ValueObject (K value)
    {
        Value = value;
    }
    public virtual K Value { get; protected set; }

    public static implicit operator K (ValueObject<T, K> x) => x.Value;

    protected override bool EqualsCore (T other)
    {
        return Value.Equals(other.Value);
    }

    protected override int GetHashCodeCore ()
    {
        return Value.GetHashCode() ^ 583;
    }

    public override string ToString ()
    {
        return Value.ToString()!;
    }
}
