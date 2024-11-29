using PieceOfCake.Application.Common.Services;
using PieceOfCake.Application.DishFeature.Dtos;

namespace PieceOfCake.Application.DishFeature.Services;

public interface IMealOfTheDayTypeService : 
    ICreateAndUpdateService<
        MealOfTheDayTypeCoreDto,
        MealOfTheDayTypeCreateCoreDto,
        MealOfTheDayTypeUpdateCoreDto>
{
}
