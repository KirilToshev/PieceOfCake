#pragma warning disable 8602
namespace PieceOfCake.Core.ValueObjects
{
    public abstract class ValueObject<T, K> : CSharpFunctionalExtensions.ValueObject<T>
        where T : ValueObject<T, K>
    {
        protected ValueObject(K value)
        {
            this.Value = value;
        }

        public virtual K Value { get; protected set; }

        protected override bool EqualsCore(T other)
        {
            return Value.Equals(other.Value);
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode() ^ 583;
        }
    }
}
