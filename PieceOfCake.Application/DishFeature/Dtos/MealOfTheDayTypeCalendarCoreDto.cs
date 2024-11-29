using PieceOfCake.Application.Common.Dtos;

namespace PieceOfCake.Application.DishFeature.Dtos;

public record MealOfTheDayTypeCalendarCoreDto : IdNameCoreDto<Guid>
{
    public required IEnumerable<DishInCalenderCoreDto> Dishes { get; init; }
}
