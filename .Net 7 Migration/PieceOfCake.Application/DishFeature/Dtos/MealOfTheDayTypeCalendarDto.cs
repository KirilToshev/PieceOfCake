using PieceOfCake.Application.Common.Dtos;

namespace PieceOfCake.Application.DishFeature.Dtos;

public record MealOfTheDayTypeCalendarDto : IdNameDto<Guid>
{
    public required IEnumerable<DishInCalenderDto> Dishes { get; init; }
}
