namespace PieceOfCake.Application.DishFeature.Dtos;

public record DishUpdateCoreDto : DishCreateCoreDto
{
    public required Guid Id { get; init; }
}
