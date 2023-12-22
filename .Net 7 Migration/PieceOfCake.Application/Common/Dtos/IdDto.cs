namespace PieceOfCake.Application.Common.Dtos;

public record IdDto<TId>
{
    public required TId Id { get; init; }
}
