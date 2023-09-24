namespace PieceOfCake.Application.Common.Dtos;

public record IdDto<TId>
{
    public TId? Id { get; init; }
}