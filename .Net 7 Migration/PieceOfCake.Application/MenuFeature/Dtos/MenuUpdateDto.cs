namespace PieceOfCake.Application.MenuFeature.Dtos;

public record MenuUpdateDto : MenuCreateDto
{
    public required Guid Id { get; init; }
}
