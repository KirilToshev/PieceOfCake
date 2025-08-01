namespace PieceOfCake.DTOs.Common;

public record IdNameDto<TId> : IdDto<TId>
{
    public required string Name { get; init; }
}
