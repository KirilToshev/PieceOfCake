using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Application.IngredientFeature.Services;
using PieceOfCake.Application.IngredientFeature.Dtos;
using PieceOfCake.Application.DishFeature.Dtos;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.ValueObjects;
using System.Linq;
using PieceOfCake.Application.Common.Services;
using PieceOfCake.Application.DishFeature.Dtos.Mapping;
using PieceOfCake.Core.Common.ValueObjects;
using System.Xml.Linq;

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

    public async Task<Result<DishDto>> CreateAsync(DishCreateDto createDto)
    {
        return await ValidateInputs(
            createDto,
            Dish.Create)
            .Map(async dish =>
            {
                UnitOfWork.DishRepository.Insert(dish);
                await UnitOfWork.SaveAsync();
                return dish.MapToGetDto();
            });
    }

    public async Task<Result<DishDto>> UpdateAsync(DishUpdateDto updateDto)
    {
        return await GetEntityAsync(updateDto.Id)
            .Bind(async dish =>
            {
                var createDto = new DishCreateDto
                {
                    Name = updateDto.Name,
                    ServingSize = updateDto.ServingSize,
                    Description = updateDto.Description,
                    MealOfTheDayTypes = updateDto.MealOfTheDayTypes,
                    IngredientsDtos = updateDto.IngredientsDtos
                };
                var result = await ValidateInputs(createDto, dish.Update)
                    .Tap(dish =>
                    {
                        Repository.Update(dish);
                        UnitOfWork.SaveAsync();
                    })
                    .Map(dish => dish.MapToGetDto());
                return result;
            });
    }

    public async Task<IReadOnlyCollection<DishDto>> GetAllAsync()
    {
        var dishes = await Repository.GetAsync();
        return dishes.Select(d => d.MapToGetDto()).ToArray().AsReadOnly();
    }

    public async Task<Result<DishDto>> GetByIdAsync(Guid id)
    {
        return await GetEntityAsync(id).Map(dish => dish.MapToGetDto());
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        return await GetEntityAsync(id)
            .Tap(async dish =>
            {
                if (dish.Menus.Any())
                {
                    Result.Failure(I18N.GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.Dish));
                }
                Repository.Delete(dish);
                await UnitOfWork.SaveAsync();
            });
    }

    private async Task<Result<Dish>> ValidateInputs(
        DishCreateDto createDto,
        Func<string, string, byte, IEnumerable<MealOfTheDayType>, IEnumerable<Ingredient>, IResources, Result<Dish>> callbackCreateFunc)
    {
        //TODO: Implement cacheing
        var measureUnitIds = createDto.IngredientsDtos.Select(x => x.MeasureUnitId).Distinct();
        var productIds = createDto.IngredientsDtos.Select(x => x.ProductId).Distinct();
        var mealTypeIds = createDto.MealOfTheDayTypes.Select(x => x.Id).Distinct();

        //TODO: Make these tree call paralel. Task.WhenAll()
        var measureUnitEntities = await UnitOfWork.MeasureUnitRepository
            .GetAsync(x => measureUnitIds.Contains(x.Id));
        var productEntities = await UnitOfWork.ProductRepository
            .GetAsync(x => productIds.Contains(x.Id));
        var mealTypeEntities = await UnitOfWork.MealOfTheDayTypeRepository
            .GetAsync(x => mealTypeIds.Contains(x.Id));

        var errors = new List<string>();

        if(measureUnitEntities.Count() < measureUnitIds.Count())
        {
            var invalidIds = measureUnitIds.Except(measureUnitEntities.Select(x => x.Id));
            errors.Add(I18N.GenereteSentence(
                    x => x.UserErrors.IdNotFound,
                    x => string.Join("; ", invalidIds)));
        }

        if(productEntities.Count() < productIds.Count())
        {
            var invalidIds = productIds.Except(productEntities.Select(x => x.Id));
            errors.Add(I18N.GenereteSentence(
                    x => x.UserErrors.IdNotFound,
                    x => string.Join("; ", invalidIds)));
        }

        if(mealTypeEntities.Count() < mealTypeIds.Count())
        {
            var invalidIds = mealTypeIds.Except(mealTypeEntities.Select(x => x.Id));
            errors.Add(I18N.GenereteSentence(
                    x => x.UserErrors.IdNotFound,
                    x => string.Join("; ", invalidIds)));
        }

        if(errors.Any())
            return Result.Failure<Dish>(string.Join("; ", errors));

        var ingredients = new List<Ingredient>(20);

        var mappedMealOfTheDayTypes = mealTypeEntities
            .IntersectBy(createDto.MealOfTheDayTypes.Select(x => x.Id), x => x.Id);

        foreach(var ingredientDto in createDto.IngredientsDtos)
        {
            var measureUnit = measureUnitEntities.First(mu => mu.Id == ingredientDto.MeasureUnitId);
            var product = productEntities.First(product => product.Id == ingredientDto.ProductId);

            var ingredientResult = Ingredient.Create(ingredientDto.Quantity, measureUnit!, product!, I18N);
            if(ingredientResult.IsFailure)
            {
                errors.Add(ingredientResult.Error);
                continue;
            }

            ingredients.Add(ingredientResult.Value);
        }

        return callbackCreateFunc(
            createDto.Name,
            createDto.Description,
            createDto.ServingSize,
            mappedMealOfTheDayTypes,
            ingredients,
            I18N);
    }
}
