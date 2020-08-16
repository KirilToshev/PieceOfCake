using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PieceOfCake.Api.Models.Dish;
using PieceOfCake.Core.DomainServices.Interfaces;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Api.Controllers
{
    [ApiController]
    [Route("api/dishes")]
    public class DishController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IResources _resources;
        private readonly IDishDomainService _dishDomainService;
        private readonly IMeasureUnitDomainService _measureUnitDomainService;
        private readonly IProductDomainService _productDomainService;

        public DishController(
            ILogger<ProductController> logger,
            IResources resources,
            IDishDomainService dishDomainService,
            IMeasureUnitDomainService measureUnitDomainService,
            IProductDomainService productDomainService
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _dishDomainService = dishDomainService ?? throw new ArgumentNullException(nameof(dishDomainService));
            _measureUnitDomainService = measureUnitDomainService ?? throw new ArgumentNullException(nameof(measureUnitDomainService));
            _productDomainService = productDomainService ?? throw new ArgumentNullException(nameof(productDomainService));
        }

        [HttpGet]
        public ActionResult<IReadOnlyCollection<Product>> Get()
        {
            var result = _dishDomainService.Get();
            if (result.IsFailure)
                return Error<IReadOnlyCollection<Product>>(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {

            var result = _dishDomainService.Get(id);
            if (result.IsFailure)
                return Error<Product>(result.Error);

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public ActionResult<Product> Put(int id, [FromBody]CreateDishVm dishVm)
        {
            var result = _dishDomainService.UpdateNameAndDescritption(id, dishVm.Name, dishVm.Description);
            if (result.IsFailure)
                return Error<Product>(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public ActionResult<Product> Post([FromBody]CreateDishVm dishVm)
        {
            var result = _dishDomainService.Create(dishVm.Name, dishVm.Description);
            if (result.IsFailure)
                return Error<Product>(result.Error);

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

        [HttpPut("{id}/test")]
        public IActionResult AddIngredients(long id, [FromBody]IEnumerable<IngredientVm> ingredientsVmList)
        {
            var errors = new List<string>();

            //TODO: Mapping should be done automaticaly and in a separate location
            var ingredients = ingredientsVmList.Select(vm =>
            {
                bool containErrors = false;

                var measureUnitResult = _measureUnitDomainService.Get(vm.MeasureUnitId);
                if (measureUnitResult.IsFailure)
                {
                    errors.Add(measureUnitResult.Error);
                    containErrors = true;
                }
                    
                var productResult = _productDomainService.Get(vm.ProductId);
                if (productResult.IsFailure)
                {
                    errors.Add(productResult.Error);
                    containErrors = true;
                }

                if (containErrors)
                    return null;

                var ingredientResult = Ingredient.Create(vm.Quantity, measureUnitResult.Value, productResult.Value, _resources);
                if (ingredientResult.IsFailure)
                {
                    errors.Add(ingredientResult.Error);
                    containErrors = true;
                }

                if (containErrors)
                    return null;

                return ingredientResult.Value;
            });

            if (errors.Any())
                return Error(errors.Aggregate((curr, next) => curr + ";" + next));

            var addResult = _dishDomainService.AddIngredients(id, ingredients.OfType<Ingredient>());
            if (addResult.IsFailure)
                return Error(addResult.Error);

            return Ok();
        }
    }
}
