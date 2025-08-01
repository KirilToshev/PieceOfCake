using PieceOfCake.DTOs.Common;
using PieceOfCake.DTOs.DishFeature;

namespace PieceOfCake.DTOs.MenuFeature;

public record MenuGetDto : IdDto<Guid>
{
    public required DateOnly StartDate { get; init; }
    public required DateOnly EndDate { get; init; }
    public required int DaysDifference { get; init; }
    public required ushort NumberOfPeople { get; init; }
    public IEnumerable<MealOfTheDayTypeGetDto> MealOfTheDayTypes { get; init; } = Enumerable.Empty<MealOfTheDayTypeGetDto>();
    public IEnumerable<CalendarItemDto> CalendarItems { get; init; } = Enumerable.Empty<CalendarItemDto>();
}
