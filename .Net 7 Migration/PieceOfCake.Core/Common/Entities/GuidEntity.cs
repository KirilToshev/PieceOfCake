using CSharpFunctionalExtensions;

namespace PieceOfCake.Core.Common.Entities;
public class GuidEntity : Entity<Guid>
{
    public new Guid Id { get; init; }
}
