using PieceOfCake.DTOs.Common;

namespace PieceOfCake.DTOs.DishFeature;

public record MealOfTheDayTypeCalendarDto : IdNameDto<Guid>
{
    public required IEnumerable<DishInCalendarDto> Dishes { get; init; }
}
