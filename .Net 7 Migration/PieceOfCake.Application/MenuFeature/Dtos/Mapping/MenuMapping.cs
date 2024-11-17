using PieceOfCake.Application.DishFeature.Dtos.Mapping;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Entities;

namespace PieceOfCake.Application.MenuFeature.Dtos.Mapping;

public static class MenuMapping
{
    public static MenuGetDto MapToGetDto(this Menu menu,
        IEnumerable<MealOfTheDayType> mealOfTheDayTypesList,
        IEnumerable<Dish> dishesList)
    {
        return new MenuGetDto
        {
            Id = menu.Id,
            StartDate = menu.Duration.StartDate,
            EndDate = menu.Duration.EndDate,
            DaysDifference = menu.Duration.DaysDifference,
            NumberOfPeople = menu.NumberOfPeople,
            MealOfTheDayTypes = menu.MealOfTheDayTypes.Select(mt => mt.MapToGetDto()),
            CalendarItems = menu.Calendar.Select(c => c.MapToDto(mealOfTheDayTypesList, dishesList))
        };
    }
}
