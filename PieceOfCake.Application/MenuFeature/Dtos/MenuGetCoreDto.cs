using PieceOfCake.Application.Common.Dtos;
using PieceOfCake.Application.DishFeature.Dtos;

namespace PieceOfCake.Application.MenuFeature.Dtos;

public record MenuGetDto : IdCoreDto<Guid>
{
    public required DateOnly StartDate { get; init; }
    public required DateOnly EndDate { get; init; }
    public required int DaysDifference { get; init; }
    public required ushort NumberOfPeople { get; init; }
    public IEnumerable<MealOfTheDayTypeCoreDto> MealOfTheDayTypes { get; init; } = Enumerable.Empty<MealOfTheDayTypeCoreDto>();
    public IEnumerable<CalendarItemCoreDto> CalendarItems { get; init; } = Enumerable.Empty<CalendarItemCoreDto>(); 
}
