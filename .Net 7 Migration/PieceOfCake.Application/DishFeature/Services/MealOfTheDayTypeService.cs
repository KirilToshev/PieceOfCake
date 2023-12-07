using CSharpFunctionalExtensions;
using PieceOfCake.Application.Common.Services;
using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.Application.DishFeature.Services;

public class MealOfTheDayTypeService : IMealOfTheDayTypeService
{
    //TODO: Implement Service
    public Result<MealOfTheDayType> Create (string name)
    {
        throw new NotImplementedException();
    }

    public Result Delete (Guid id)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<MealOfTheDayType> Get ()
    {
        throw new NotImplementedException();
    }

    public Result<MealOfTheDayType> Get (Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<MealOfTheDayType> Update (Guid id, string name)
    {
        throw new NotImplementedException();
    }
}
