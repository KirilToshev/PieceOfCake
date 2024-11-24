namespace PieceOfCake.Application.MenuFeature.Dtos;

public record MenuCreateDto
{
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required ushort NumberOfPeople { get; init; }
    public IEnumerable<Guid> MealOfTheDayTypes { get; init; } = Enumerable.Empty<Guid>();
}
