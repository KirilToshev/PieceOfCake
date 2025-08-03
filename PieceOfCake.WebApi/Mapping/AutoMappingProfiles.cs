using AutoMapper;
using PieceOfCake.Application.Common.Dtos;
using PieceOfCake.Application.DishFeature.Dtos;
using PieceOfCake.Application.IngredientFeature.Dtos;
using PieceOfCake.Application.MenuFeature.Dtos;
using PieceOfCake.DTOs.Common;
using PieceOfCake.DTOs.DishFeature;
using PieceOfCake.DTOs.IngredientFeature;
using PieceOfCake.DTOs.MenuFeature;

namespace PieceOfCake.WebApi.Mapping;

public class AutoMappingProfiles : Profile
{
    public AutoMappingProfiles()
    {
        #region Request mappings

        CreateMap(typeof(IdCoreDto<>), typeof(IdDto<>)).ReverseMap();
        CreateMap(typeof(IdNameCoreDto<>), typeof(IdNameDto<>)).ReverseMap();
        CreateMap<ProductCreateDto, ProductCreateCoreDto>();
        CreateMap<ProductUpdateDto, ProductUpdateCoreDto>();
        CreateMap<MeasureUnitCreateDto, MeasureUnitCreateCoreDto>();
        CreateMap<MeasureUnitUpdateDto, MeasureUnitUpdateCoreDto>();
        CreateMap<MealOfTheDayTypeCreateDto, MealOfTheDayTypeCreateCoreDto>();
        CreateMap<MealOfTheDayTypeUpdateDto, MealOfTheDayTypeUpdateCoreDto>();
        CreateMap<DishCreateDto, DishCreateCoreDto>();
        CreateMap<DishUpdateDto, DishUpdateCoreDto>();
        CreateMap<IngredientCreateDto, IngredientCreateCoreDto>();
        CreateMap<MenuCreateDto, MenuCreateCoreDto>();
        CreateMap<MenuUpdateDto, MenuUpdateCoreDto>();
        #endregion

        #region Response mappings

        CreateMap<ProductGetCoreDto, ProductGetDto>();
        CreateMap<MeasureUnitGetCoreDto, MeasureUnitGetDto>();
        CreateMap<MealOfTheDayTypeCoreDto, MealOfTheDayTypeGetDto>();
        CreateMap<DishCoreDto, DishDto>();
        CreateMap<MenuGetCoreDto, MenuGetDto>();
        CreateMap<CalendarItemCoreDto, CalendarItemDto>();
        CreateMap<MealOfTheDayTypeCalendarCoreDto, MealOfTheDayTypeCalendarDto>();
        CreateMap<DishInCalendarCoreDto, DishInCalendarDto>();

        #endregion
    }
}
