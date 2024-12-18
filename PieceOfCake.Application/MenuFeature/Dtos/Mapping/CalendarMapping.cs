﻿using PieceOfCake.Application.DishFeature.Dtos;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.MenuFeature.Calendar;

namespace PieceOfCake.Application.MenuFeature.Dtos.Mapping;

public static class CalendarMapping
{
    public static CalendarItemDto MapToDto(this CalendarItem calendar, 
        IEnumerable<MealOfTheDayType> mealOfTheDayTypesList,
        IEnumerable<Dish> dishesList)
    {
        var mealTypes = mealOfTheDayTypesList.ToDictionary(x => x.Id);
        var dishes = dishesList.ToDictionary(x => x.Id);
        return new CalendarItemDto
        {
            Date = calendar.Date,
            MealOfTheDayTypeDtos = calendar.MealOfTheDayTypes.Select(mt =>
            {
                return new MealOfTheDayTypeCalendarDto
                {
                    Id = mt.Id,
                    Name = mealTypes[mt.Id].Name,
                    Dishes = mt.Dishes.Select(d =>
                    {
                        return new DishInCalenderDto
                        {
                            Id = d.Id,
                            Name = dishes[d.Id].Name
                        };
                    })
                };
            })
        };
    }
}
