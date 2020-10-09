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
using PieceOfCake.Core.IoModels;

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
        
        
        public DishController(
            ILogger<ProductController> logger,
            IResources resources,
            IDishDomainService dishDomainService,
            
            IMapper mapper
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _dishDomainService = dishDomainService ?? throw new ArgumentNullException(nameof(dishDomainService));
            
        }

        [HttpGet]
        public ActionResult<IReadOnlyCollection<DishVm>> Get()
        {
            var result = _dishDomainService.Get();
            if (result.IsFailure)
                return Error<IReadOnlyCollection<DishVm>>(result.Error);

            return Ok(result.Value.Select(x => _mapper.Map<DishVm>(x)));
        }

        [HttpGet("{id}")]
        public ActionResult<DishVm> Get(int id)
        {

            var result = _dishDomainService.Get(id);
            if (result.IsFailure)
                return Error<DishVm>(result.Error);

            return Ok(_mapper.Map<DishVm>(result.Value));
        }

        [HttpPut("{id}")]
        public ActionResult<DishVm> Put(int id, [FromBody]UpdateDishVm dishVm)
        {
            var result = _dishDomainService.UpdateNameAndDescritption(id, dishVm.Name, dishVm.Description);
            if (result.IsFailure)
                return Error<DishVm>(result.Error);

            return Ok(_mapper.Map<DishVm>(result.Value));
        }

        [HttpPost]
        public ActionResult<DishVm> Post([FromBody]CreateDishVm dishVm)
        {
            var result = _dishDomainService.Create(dishVm.Name, dishVm.Description);
            if (result.IsFailure)
                return Error<DishVm>(result.Error);

            return Ok(_mapper.Map<DishVm>(result.Value));
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
        public IActionResult UpdateDishIngredients(long id, [FromBody]IEnumerable<AddIngredientVm> ingredientsVmList)
        {
            var ingredients = _mapper.ProjectTo<AddIngredientDto>(ingredientsVmList.AsQueryable());
            var result = _dishDomainService.UpdateIngredients(id, ingredients);
            if (result.IsFailure)
                return Error(result.Error);

            return Ok();
        }
    }
}
