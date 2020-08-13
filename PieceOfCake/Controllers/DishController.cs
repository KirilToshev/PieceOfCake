using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PieceOfCake.Api.Models.Dish;
using PieceOfCake.Core.DomainServices.Interfaces;
using PieceOfCake.Core.Entities;

namespace PieceOfCake.Api.Controllers
{
    [ApiController]
    [Route("api/dishes")]
    public class DishController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IDishDomainService _dishDomainService;

        public DishController(
            ILogger<ProductController> logger,
            IDishDomainService productDomainService
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dishDomainService = productDomainService ?? throw new ArgumentNullException(nameof(productDomainService));
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
    }
}
