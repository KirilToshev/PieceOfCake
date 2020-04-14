using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PieceOfCake.Core.BusinessRules;
using PieceOfCake.Core.Entities;

namespace PieceOfCake.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductBusinessRules _productBr;

        public ProductController(
            ILogger<ProductController> logger,
            IProductBusinessRules productBr
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productBr = productBr ?? throw new ArgumentNullException(nameof(productBr));
        }

        [HttpGet]
        public ActionResult<IReadOnlyCollection<Product>> Get()
        {
            var result = _productBr.Get();
            if (result.IsFailure)
                return Error<IReadOnlyCollection<Product>>(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {

            var result = _productBr.Get(id);
            if (result.IsFailure)
                return Error<Product>(result.Error);

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public ActionResult<Product> Put(int id, [FromBody]string name)
        {
            var result = _productBr.Update(id, name);
            if (result.IsFailure)
                return Error<Product>(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public ActionResult<Product> Post([FromBody]string name)
        {
            var result = _productBr.Create(name);
            if (result.IsFailure)
                return Error<Product>(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var result = _productBr.Delete(id);
            if (result.IsFailure)
                return Error(result.Error);

            return Ok();
        }
    }
}
