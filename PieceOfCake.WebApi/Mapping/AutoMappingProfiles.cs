using AutoMapper;
using PieceOfCake.Application.Common.Dtos;
using PieceOfCake.Application.DishFeature.Dtos;
using PieceOfCake.Application.IngredientFeature.Dtos;
using PieceOfCake.DTOs.Common;
using PieceOfCake.DTOs.IngredientFeature;

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
        #endregion

        #region Response mappings

        CreateMap<ProductGetCoreDto, ProductGetDto>();
        CreateMap<MeasureUnitGetCoreDto, MeasureUnitGetDto>();
        CreateMap<MealOfTheDayTypeCoreDto, MealOfTheDayTypeGetDto>();

        #endregion
    }
}
