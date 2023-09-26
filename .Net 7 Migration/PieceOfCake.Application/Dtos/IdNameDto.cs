namespace PieceOfCake.Application.Dtos;

public record IdNameDto<TId> : IdDto<TId>
{
    public required string Name { get; init; }
}
