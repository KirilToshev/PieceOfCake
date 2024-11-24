using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Application.DishFeature.Dtos;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.ValueObjects;
using PieceOfCake.Application.Common.Services;
using PieceOfCake.Application.DishFeature.Dtos.Mapping;

namespace PieceOfCake.Application.DishFeature.Services;

public class DishService : BaseService<IDishRepository, Dish>, IDishService
{
    protected override IDishRepository Repository => UnitOfWork.DishRepository;

    public DishService(
        IResources resources,
        IUnitOfWork unitOfWork)
        : base(resources, unitOfWork)
    {
    }

    public async Task<Result<DishDto>> CreateAsync(DishCreateDto createDto, CancellationToken cancellationToken)
    {
        return await ValidateInputs(createDto, Dish.Create, cancellationToken)
            .Map(async dish =>
            {
                UnitOfWork.DishRepository.Insert(dish);
                await UnitOfWork.SaveAsync(cancellationToken);
                return dish.MapToGetDto();
            });
    }

    public async Task<Result<DishDto>> UpdateAsync(DishUpdateDto updateDto, CancellationToken cancellationToken)
    {
        return await GetEntityAsync(updateDto.Id, cancellationToken)
            .Bind(async dish =>
            {
                var result = await ValidateInputs(updateDto, dish.Update, cancellationToken)
                    .Tap(dish =>
                    {
                        Repository.Update(dish);
                        UnitOfWork.SaveAsync(cancellationToken);
                    })
                    .Map(dish => dish.MapToGetDto());
                return result;
            });
    }

    public async Task<IReadOnlyCollection<DishDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var dishes = await Repository.GetAsync(cancellationToken);
        return dishes.Select(d => d.MapToGetDto()).ToArray().AsReadOnly();
    }

    public async Task<Result<DishDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await GetEntityAsync(id, cancellationToken).Map(dish => dish.MapToGetDto());
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await GetEntityAsync(id, cancellationToken)
            .Bind(async dish =>
            {
                var usedInMenu = await UnitOfWork.MenuRepository
                                    .FirstOrDefaultAsync(cancellationToken, 
                                        menu => menu.Dishes.Contains(dish), null);
                if(usedInMenu is not null)
                    return Result.Failure(I18N.GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.Dish));
                
                Repository.Delete(dish);
                await UnitOfWork.SaveAsync(cancellationToken);
                return Result.Success();
            });
    }

    private async Task<Result<Dish>> ValidateInputs(
        DishCreateDto createDto,
        Func<string, string, byte, IEnumerable<MealOfTheDayType>, IEnumerable<Ingredient>, IResources, Result<Dish>> callbackCreateFunc,
        CancellationToken cancellationToken)
    {
        //TODO: Implement cacheing
        var measureUnitIds = createDto.IngredientsDtos.Select(x => x.MeasureUnitId).Distinct();
        var productIds = createDto.IngredientsDtos.Select(x => x.ProductId).Distinct();
        var mealTypeIds = createDto.MealOfTheDayTypeIds.Distinct();

        //TODO: Implement Specification pattern.
        var measureUnitEntities = await UnitOfWork.MeasureUnitRepository
            .GetAsync(cancellationToken, x => measureUnitIds.Contains(x.Id));
        var productEntities = await UnitOfWork.ProductRepository
            .GetAsync(cancellationToken, x => productIds.Contains(x.Id));
        var mealTypeEntities = await UnitOfWork.MealOfTheDayTypeRepository
            .GetAsync(cancellationToken, x => mealTypeIds.Contains(x.Id));

        var errors = new List<string>();

        var invalidMeasureUnitIds = measureUnitIds.Except(measureUnitEntities.Select(x => x.Id));
        if(invalidMeasureUnitIds.Any())
        {
            errors.Add(I18N.GenereteSentence(
                x => x.UserErrors.IdNotFound,
                x => string.Join("; ", invalidMeasureUnitIds)));
        }
        
        var invalidProductIds = productIds.Except(productEntities.Select(x => x.Id));
        if(invalidProductIds.Any())
        {
            errors.Add(I18N.GenereteSentence(
                x => x.UserErrors.IdNotFound,
                x => string.Join("; ", invalidProductIds)));
        }
        
        var invalidMealTypeIds = mealTypeIds.Except(mealTypeEntities.Select(x => x.Id));
        if(invalidMealTypeIds.Any())
        {
            errors.Add(I18N.GenereteSentence(
                x => x.UserErrors.IdNotFound,
                x => string.Join("; ", invalidMealTypeIds)));
        }

        if(errors.Any())
            return Result.Failure<Dish>(string.Join("; ", errors));

        var ingredients = new List<Ingredient>(20);

        var mappedMealOfTheDayTypes = mealTypeEntities
            .IntersectBy(createDto.MealOfTheDayTypeIds, x => x.Id);

        foreach(var ingredientDto in createDto.IngredientsDtos)
        {
            var measureUnit = measureUnitEntities.First(mu => mu.Id == ingredientDto.MeasureUnitId);
            var product = productEntities.First(product => product.Id == ingredientDto.ProductId);

            var ingredientResult = Ingredient.Create(ingredientDto.Quantity, measureUnit, product, I18N);
            if(ingredientResult.IsFailure)
            {
                errors.Add(ingredientResult.Error);
                continue;
            }

            ingredients.Add(ingredientResult.Value);
        }

        if(errors.Any())
            return Result.Failure<Dish>(string.Join("; ", errors));

        return callbackCreateFunc(
            createDto.Name,
            createDto.Description,
            createDto.ServingSize,
            mappedMealOfTheDayTypes,
            ingredients,
            I18N);
    }
}
