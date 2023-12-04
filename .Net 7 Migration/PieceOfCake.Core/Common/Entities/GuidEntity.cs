using CSharpFunctionalExtensions;

namespace PieceOfCake.Core.Common.Entities;
public class GuidEntity : Entity<Guid>
{
    public GuidEntity() :base(Guid.NewGuid())
    {
    }
}
