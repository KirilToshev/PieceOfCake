namespace PieceOfCake.Application.Common.Dtos;

public record IdNameDto<TId> : IdDto<TId>
{
    public required string Name { get; init; }
}
