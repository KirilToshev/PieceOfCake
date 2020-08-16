using System;

namespace PieceOfCake.Core.Common
{
    public abstract class Entity
    {
        public long Id { get; }

        protected Entity()
        {
        }

        protected Entity(long id)
            : this()
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Entity other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetRealType() != other.GetRealType())
                return false;

            if (Id == 0 || other.Id == 0)
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(Entity? a, Entity? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity? a, Entity? b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetRealType().ToString() + Id).GetHashCode();
        }

        protected Type GetRealType()
        {
            Type type = GetType();

            if (type.ToString().Contains("Castle.Proxies.")) //Separation of conserns violated. This code is because of EF Core.
                return type.BaseType ?? throw new ArgumentNullException(nameof(type.BaseType));

            return type;
        }
    }
}
