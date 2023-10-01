using CSharpFunctionalExtensions;

namespace PieceOfCake.Core.Common.Entities;
public class GuidEntity : Entity<Guid>
{
    public GuidEntity ()
    {
        Id = Guid.NewGuid ();
    }

    public new Guid Id { get; init; }
}
