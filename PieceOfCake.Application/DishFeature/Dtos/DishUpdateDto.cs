namespace PieceOfCake.Application.DishFeature.Dtos;

public record DishUpdateDto : DishCreateDto
{
    public required Guid Id { get; init; }
}
