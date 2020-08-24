using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PieceOfCake.Shared.ViewModels.Dish.Ingredient;
using PieceOfCake.Core.DomainServices.Interfaces;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Resources;
using AutoMapper;
using PieceOfCake.Shared.ViewModels.Dish;

namespace PieceOfCake.Api.Controllers
{
    [ApiController]
    [Route("api/dishes")]
    public class DishController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IResources _resources;
        private readonly IMapper _mapper;

        private readonly IDishDomainService _dishDomainService;
        private readonly IMeasureUnitDomainService _measureUnitDomainService;
        private readonly IProductDomainService _productDomainService;
        

        public DishController(
            ILogger<ProductController> logger,
            IResources resources,
            IDishDomainService dishDomainService,
            IMeasureUnitDomainService measureUnitDomainService,
            IProductDomainService productDomainService,
            IMapper mapper
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _dishDomainService = dishDomainService ?? throw new ArgumentNullException(nameof(dishDomainService));
            _measureUnitDomainService = measureUnitDomainService ?? throw new ArgumentNullException(nameof(measureUnitDomainService));
            _productDomainService = productDomainService ?? throw new ArgumentNullException(nameof(productDomainService));
        }

        [HttpGet]
        public ActionResult<IReadOnlyCollection<Dish>> Get()
        {
            var result = _dishDomainService.Get();
            if (result.IsFailure)
                return Error<IReadOnlyCollection<Dish>>(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public ActionResult<Dish> Get(int id)
        {

            var result = _dishDomainService.Get(id);
            if (result.IsFailure)
                return Error<Dish>(result.Error);

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public ActionResult<Dish> Put(int id, [FromBody]UpdateDishVm dishVm)
        {
            var result = _dishDomainService.UpdateNameAndDescritption(id, dishVm.Name, dishVm.Description);
            if (result.IsFailure)
                return Error<Dish>(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public ActionResult<Dish> Post([FromBody]CreateDishVm dishVm)
        {
            var result = _dishDomainService.Create(dishVm.Name, dishVm.Description);
            if (result.IsFailure)
                return Error<Dish>(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var result = _dishDomainService.Delete(id);
            if (result.IsFailure)
                return Error(result.Error);

            return Ok();
        }

        [HttpPatch("{id}")]
        public IActionResult AddIngredients(long id, [FromBody]IEnumerable<AddIngredientVm> ingredientsVmList)
        {
            var errors = new List<string>();
            bool containErrors = false;
            var ingredients = new List<Ingredient>();

            foreach (var ingredientVm in ingredientsVmList)
            {
                var measureUnitResult = _measureUnitDomainService.Get(ingredientVm.MeasureUnitId);
                if (measureUnitResult.IsFailure)
                {
                    errors.Add(measureUnitResult.Error);
                    containErrors = true;
                }

                var productResult = _productDomainService.Get(ingredientVm.ProductId);
                if (productResult.IsFailure)
                {
                    errors.Add(productResult.Error);
                    containErrors = true;
                }

                if (containErrors)
                    continue;

                var ingredientResult = Ingredient.Create(ingredientVm.Quantity, measureUnitResult.Value, productResult.Value, _resources);
                if (ingredientResult.IsFailure)
                {
                    errors.Add(ingredientResult.Error);
                    continue;
                }

                ingredients.Add(ingredientResult.Value);
            }

            if (errors.Any())
                return Error(errors.Aggregate((curr, next) => curr + ";" + next));

            var addResult = _dishDomainService.AddIngredients(id, ingredients);
            if (addResult.IsFailure)
                return Error(addResult.Error);

            return Ok();
        }
    }
}
