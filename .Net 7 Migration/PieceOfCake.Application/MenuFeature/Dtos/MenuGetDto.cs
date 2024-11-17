using PieceOfCake.Application.Common.Dtos;
using PieceOfCake.Application.DishFeature.Dtos;

namespace PieceOfCake.Application.MenuFeature.Dtos;

public record MenuGetDto : IdDto<Guid>
{
    public required DateOnly StartDate { get; init; }
    public required DateOnly EndDate { get; init; }
    public required int DaysDifference { get; init; }
    public required ushort NumberOfPeople { get; init; }
    public IEnumerable<MealOfTheDayTypeDto> MealOfTheDayTypes { get; init; } = Enumerable.Empty<MealOfTheDayTypeDto>();
    public IEnumerable<CalendarItemDto> CalendarItems { get; init; } = Enumerable.Empty<CalendarItemDto>(); 
}
