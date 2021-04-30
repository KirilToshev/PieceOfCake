using CSharpFunctionalExtensions;
using PieceOfCake.Core.DomainServices.Interfaces;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.IoModels;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PieceOfCake.Core.DomainServices
{
    public class DishDomainService : IDishDomainService
    {
        private readonly IResources _resources;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMeasureUnitDomainService _measureUnitDomainService;
        private readonly IProductDomainService _productDomainService;

        public DishDomainService(
            IResources resources,
            IUnitOfWork unitOfWork,
            IMeasureUnitDomainService measureUnitDomainService,
            IProductDomainService productDomainService)
        {
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _measureUnitDomainService = measureUnitDomainService ?? throw new ArgumentNullException(nameof(measureUnitDomainService));
            _productDomainService = productDomainService ?? throw new ArgumentNullException(nameof(productDomainService));
        }


        public Result<IReadOnlyCollection<Dish>> Get()
        {
            var dishes = _unitOfWork.DishRepository.Get();

            if (!dishes.Any())
            {
                return Result.Failure<IReadOnlyCollection<Dish>>(
                    _resources.GenereteSentence(x => x.UserErrors.SequenceContainsNoElements));
            }

            return Result.Success(dishes);
        }

        public Result<Dish> Get(long id)
        {
            var dish = _unitOfWork.DishRepository.GetById(id);

            if (dish == null)
                return Result.Failure<Dish>(
                    _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

            return Result.Success(dish);
        }

        public Result<Dish> UpdateNameAndDescritption(long id, string? name, string? description)
        {
            var dishResult = this.Get(id);
            if (dishResult.IsFailure)
                return dishResult;

            return dishResult.Value.UpdateNameAndDescritption(name, description, _resources)
                .Tap(dish =>
                {
                    _unitOfWork.DishRepository.Update(dish);
                    _unitOfWork.Save();
                });
        }

        public Result<Dish> Create(string? name, string? description)
        {
            return Dish.Create(name, description, _resources)
                .Tap(dish =>
                {
                    _unitOfWork.DishRepository.Insert(dish);
                    _unitOfWork.Save();
                });
        }

        public Result Delete(long id)
        {
            return this.Get(id)
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

        public Result UpdateIngredients(long id, IEnumerable<AddIngredientDto> ingredientsVmList)
        {
            var errors = new List<string>();
            bool containErrors = false;
            var ingredients = new List<Ingredient>();

            foreach (var ingredientDto in ingredientsVmList)
            {
                var measureUnitResult = _measureUnitDomainService.Get(ingredientDto.MeasureUnitId);
                if (measureUnitResult.IsFailure)
                {
                    errors.Add(measureUnitResult.Error);
                    containErrors = true;
                }

                var productResult = _productDomainService.Get(ingredientDto.ProductId);
                if (productResult.IsFailure)
                {
                    errors.Add(productResult.Error);
                    containErrors = true;
                }

                if (containErrors)
                    continue;

                var ingredientResult = Ingredient.Create(ingredientDto.Quantity, measureUnitResult.Value, productResult.Value, _resources);
                if (ingredientResult.IsFailure)
                {
                    errors.Add(ingredientResult.Error);
                    continue;
                }

                ingredients.Add(ingredientResult.Value);
            }

            if (errors.Any())
                return Result.Failure(errors.Aggregate((curr, next) => curr + ";" + next));

            return Get(id)
                .Tap(dish => dish.UpdateIngredients(ingredients, _resources)
                .Tap(() => {
                    _unitOfWork.DishRepository.Update(dish);
                    _unitOfWork.Save();
                }));
        }
    }
}
