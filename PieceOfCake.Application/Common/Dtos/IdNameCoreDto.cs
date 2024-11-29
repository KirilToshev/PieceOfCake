namespace PieceOfCake.Application.Common.Dtos;

public record IdNameCoreDto<TId> : IdCoreDto<TId>
{
    public required string Name { get; init; }
}
