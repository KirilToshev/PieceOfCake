namespace PieceOfCake.Core.Dtos;

public record IdDto<TId>
{
    public TId? Id { get; init; }
}
