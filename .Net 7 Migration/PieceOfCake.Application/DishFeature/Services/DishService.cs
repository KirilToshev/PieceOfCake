using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Application.IngredientFeature.Services;
using PieceOfCake.Application.IngredientFeature.Dtos;
using PieceOfCake.Application.DishFeature.Dtos;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.ValueObjects;
using System.Linq;

namespace PieceOfCake.Application.DishFeature.Services;

public class DishService : IDishService
{
    private readonly IResources _resources;
    private readonly IUnitOfWork _unitOfWork;
    
    public DishService (
        IResources resources,
        IUnitOfWork unitOfWork)
    {
        _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));        
    }

    public IReadOnlyCollection<Dish> Get () => _unitOfWork.DishRepository.Get();

    public Result<Dish> Get (Guid id)
    {
        var dish = _unitOfWork.DishRepository.GetById(id);

        if (dish == null)
            return Result.Failure<Dish>(
                _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

        return Result.Success(dish);
    }

    public Result<Dish> Create (
        string name,
        string description,
        byte servingSize,
        IEnumerable<MealOfTheDayTypeDto> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsDtos)
    {
        return ValidateInputs(
            name, 
            description, 
            servingSize, 
            mealOfTheDayTypes, 
            ingredientsDtos, 
            Dish.Create)
            .Tap(dish =>
            {
                _unitOfWork.DishRepository.Insert(dish);
                _unitOfWork.Save();
            });
    }

    public Result<Dish> Update (
        Guid id,
        string name,
        string description,
        byte servingSize,
        IEnumerable<MealOfTheDayTypeDto> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsDtos)
    {
        var dishResult = Get(id);
        if (dishResult.IsFailure)
            return dishResult;

        return ValidateInputs(
            name, 
            description, 
            servingSize, 
            mealOfTheDayTypes, 
            ingredientsDtos, 
            dishResult.Value.Update)
            .Tap(dish =>
            {
                _unitOfWork.DishRepository.Update(dish);
                _unitOfWork.Save();
            });
    }

    public Result Delete (Guid id)
    {
        return Get(id)
            .Bind(dish =>
            {
                //TODO: Check if Dish is in use before deletion.

                var isDishInUse = _unitOfWork.DishRepository.IsDishInUse(id);
                if (isDishInUse)
                    return Result.Failure(_resources
                        .GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.Dish));

                _unitOfWork.DishRepository.Delete(dish);
                _unitOfWork.Save();
                return Result.Success();
            });
    }

    private Result<Dish> ValidateInputs (
        string name,
        string description,
        byte servingSize,
        IEnumerable<MealOfTheDayTypeDto> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsDtos,
        Func<string, string, byte, IEnumerable<MealOfTheDayType>, IEnumerable<Ingredient>, IResources, Result<Dish>> callbackCreateFunc)
    {
        //TODO: Implement cacheing
        var measureUnitIds = ingredientsDtos.Select(x => x.MeasureUnitId).Distinct();
        var productIds = ingredientsDtos.Select(x => x.ProductId).Distinct();
        var mealTypeIds = mealOfTheDayTypes.Select(x => x.Id).Distinct();

        var measureUnitEntities = _unitOfWork.MeasureUnitRepository
            .Get(x => measureUnitIds.Contains(x.Id))
            .AsEnumerable();
        var productEntities = _unitOfWork.ProductRepository
            .Get(x => productIds.Contains(x.Id));
        var mealTypeEntities = _unitOfWork.MealOfTheDayTypeRepository
            .Get(x => mealTypeIds.Contains(x.Id));
        var errors = new List<string>();
        
        if (measureUnitEntities.Count() < measureUnitIds.Count()) 
        {
            var invalidIds = measureUnitIds.Except(measureUnitEntities.Select(x => x.Id));
            errors.Add(_resources.GenereteSentence(
                    x => x.UserErrors.IdNotFound,
                    x => string.Join("; ", invalidIds)));
        }

        if (productEntities.Count() < productIds.Count())
        {
            var invalidIds = productIds.Except(productEntities.Select(x => x.Id));
            errors.Add(_resources.GenereteSentence(
                    x => x.UserErrors.IdNotFound,
                    x => string.Join("; ", invalidIds)));
        }

        if (mealTypeEntities.Count() < mealTypeIds.Count())
        {
            var invalidIds = mealTypeIds.Except(mealTypeEntities.Select(x => x.Id));
            errors.Add(_resources.GenereteSentence(
                    x => x.UserErrors.IdNotFound,
                    x => string.Join("; ", invalidIds)));
        }

        if (errors.Any())
            return Result.Failure<Dish>(string.Join("; ", errors));

        var ingredients = new List<Ingredient>(20);

        var mappedMealOfTheDayTypes = mealTypeEntities
            .IntersectBy(mealOfTheDayTypes.Select(x => x.Id), x => x.Id);

        foreach (var ingredientDto in ingredientsDtos)
        {
            var measureUnit = measureUnitEntities.First(mu => mu.Id == ingredientDto.MeasureUnitId);
            var product = productEntities.First(product => product.Id == ingredientDto.ProductId);

            var ingredientResult = Ingredient.Create(ingredientDto.Quantity, measureUnit!, product!, _resources);
            if (ingredientResult.IsFailure)
            {
                errors.Add(ingredientResult.Error);
                continue;
            }

            ingredients.Add(ingredientResult.Value);
        }

        return callbackCreateFunc(
            name, 
            description, 
            servingSize, 
            mappedMealOfTheDayTypes, 
            ingredients, 
            _resources);
    }
}
