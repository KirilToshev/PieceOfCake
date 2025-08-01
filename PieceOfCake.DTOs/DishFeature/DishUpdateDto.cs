namespace PieceOfCake.DTOs.DishFeature;

public record DishUpdateDto : DishCreateDto
{
    public required Guid Id { get; init; }
}
