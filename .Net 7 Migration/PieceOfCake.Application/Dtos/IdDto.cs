namespace PieceOfCake.Application.Dtos;

public record IdDto<TId>
{
    public TId? Id { get; init; }
}
