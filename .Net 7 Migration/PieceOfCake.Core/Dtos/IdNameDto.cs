namespace PieceOfCake.Core.Dtos;

public record IdNameDto<TId> : IdDto<TId>
{
    public required string Name { get; init; }
}
