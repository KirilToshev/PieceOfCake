namespace PieceOfCake.DTOs.Common;

public record IdDto<TId>
{
    public required TId Id { get; init; }
}
