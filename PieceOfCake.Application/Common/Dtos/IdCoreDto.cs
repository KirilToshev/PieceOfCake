namespace PieceOfCake.Application.Common.Dtos;

public record IdCoreDto<TId>
{
    public required TId Id { get; init; }
}
