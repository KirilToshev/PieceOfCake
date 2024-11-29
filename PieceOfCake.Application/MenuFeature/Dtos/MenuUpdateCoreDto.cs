namespace PieceOfCake.Application.MenuFeature.Dtos;

public record MenuUpdateCoreDto : MenuCreateCoreDto
{
    public required Guid Id { get; init; }
}
