using CSharpFunctionalExtensions;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.Dtos;
using PieceOfCake.Core.ValueObjects;
using PieceOfCake.Application.MealOfTheDayType;
using PieceOfCake.Application.MeasureUnit;
using PieceOfCake.Application.Product.Services;

namespace PieceOfCake.Application.Dish;

public class DishService : IDishService
{
    private readonly IResources _resources;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMealOfTheDayTypeService _mealOfTheDayTypeService;
    private readonly IMeasureUnitService _measureUnitDomainService;
    private readonly IProductService _productDomainService;

    public DishService (
        IResources resources,
        IUnitOfWork unitOfWork,
        IMealOfTheDayTypeService measureUnitService,
        IMeasureUnitService measureUnitDomainService,
        IProductService productDomainService)
    {
        _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mealOfTheDayTypeService = measureUnitService;
        _measureUnitDomainService = measureUnitDomainService ?? throw new ArgumentNullException(nameof(measureUnitDomainService));
        _productDomainService = productDomainService ?? throw new ArgumentNullException(nameof(productDomainService));
    }

    public IReadOnlyCollection<Core.Entities.Dish> Get () => _unitOfWork.DishRepository.Get();

    public Result<Core.Entities.Dish> Get (Guid id)
    {
        var dish = _unitOfWork.DishRepository.GetById(id);

        if (dish == null)
            return Result.Failure<Core.Entities.Dish>(
                _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

        return Result.Success(dish);
    }

    public Result<Core.Entities.Dish> Create (
        string name,
        string description,
        int servingSize,
        IEnumerable<Core.Entities.MealOfTheDayType> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsDtos)
    {
        return ValidateInputs(name, description, servingSize, mealOfTheDayTypes, ingredientsDtos, Core.Entities.Dish.Create)
            .Tap(dish =>
            {
                _unitOfWork.DishRepository.Insert(dish);
                _unitOfWork.Save();
            });
    }

    public Result<Core.Entities.Dish> Update (
        Guid id,
        string name,
        string description,
        int servingSize,
        IEnumerable<Core.Entities.MealOfTheDayType> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsDtos)
    {
        var dishResult = Get(id);
        if (dishResult.IsFailure)
            return dishResult;

        return ValidateInputs(name, description, servingSize, mealOfTheDayTypes, ingredientsDtos, dishResult.Value.Update)
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
                var isDishInUse = _unitOfWork.MenuRepository
                                    .Get(menu => menu.Dishes.Contains(dish))
                                    .Any();

                if (isDishInUse)
                    return Result.Failure(_resources
                        .GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.Dish));

                _unitOfWork.DishRepository.Delete(dish);
                _unitOfWork.Save();
                return Result.Success();
            });
    }

    private Result<Core.Entities.Dish> ValidateInputs (
        string name,
        string description,
        int servingSize,
        IEnumerable<Core.Entities.MealOfTheDayType> mealOfTheDayTypes,
        IEnumerable<AddIngredientDto> ingredientsDtos,
        Func<string, string, int, IEnumerable<Core.Entities.MealOfTheDayType>, IEnumerable<Ingredient>, IResources, Result<Core.Entities.Dish>> callbackFunc)
    {
        //TODO: Implement cacheing
        var allMeasureUnits = _measureUnitDomainService.Get();
        var allProducts = _productDomainService.Get();
        var allMealOfTheDayTypes = _mealOfTheDayTypeService.Get();
        var errors = new List<string>();
        var ingredients = new List<Ingredient>(20);

        var invalidMealOfTheDayTypes = mealOfTheDayTypes
            .Select(x => x.Id)
            .Except(allMealOfTheDayTypes.Select(y => y.Id));

        if (invalidMealOfTheDayTypes.Any())
            errors.AddRange(invalidMealOfTheDayTypes.Select(id =>
                _resources.GenereteSentence(
                    x => x.UserErrors.IdNotFound,
                    x => id.ToString())));

        foreach (var ingredientDto in ingredientsDtos)
        {
            var measureUnit = allMeasureUnits.FirstOrDefault(mu => mu.Id == ingredientDto.MeasureUnitId);
            var product = allProducts.FirstOrDefault(product => product.Id == ingredientDto.ProductId);

            if (measureUnit is null)
                errors.Add(_resources.GenereteSentence(
                    x => x.UserErrors.IdNotFound,
                    x => ingredientDto.MeasureUnitId.ToString()));

            if (product is null)
                errors.Add(_resources.GenereteSentence(
                    x => x.UserErrors.IdNotFound,
                    x => ingredientDto.ProductId.ToString()));

            if (errors.Any())
                continue;

            var ingredientResult = Ingredient.Create(ingredientDto.Quantity, measureUnit!, product!, _resources);
            if (ingredientResult.IsFailure)
            {
                errors.Add(ingredientResult.Error);
                continue;
            }

            ingredients.Add(ingredientResult.Value);
        }

        if (errors.Any())
            return Result.Failure<Core.Entities.Dish>(string.Join(";", errors));

        return callbackFunc(name, description, servingSize, mealOfTheDayTypes, ingredients, _resources);
    }
}
