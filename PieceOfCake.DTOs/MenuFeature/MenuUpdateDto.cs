namespace PieceOfCake.DTOs.MenuFeature;

public record MenuUpdateDto : MenuCreateDto
{
    public required Guid Id { get; init; }
}
