using PieceOfCake.Application.Common.Services;
using PieceOfCake.Application.DishFeature.Dtos;

namespace PieceOfCake.Application.DishFeature.Services;

public interface IDishService : ICreateAndUpdateService<DishCoreDto, DishCreateCoreDto, DishUpdateCoreDto>
{
}
