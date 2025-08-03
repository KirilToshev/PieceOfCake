using PieceOfCake.Application.Common.Dtos;

namespace PieceOfCake.Application.DishFeature.Dtos;

public record MealOfTheDayTypeCalendarCoreDto : IdNameCoreDto<Guid>
{
    public required IEnumerable<DishInCalendarCoreDto> Dishes { get; init; }
}
