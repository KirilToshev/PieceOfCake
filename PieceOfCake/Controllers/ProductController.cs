using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PieceOfCake.Core.DomainServices.Interfaces;
using PieceOfCake.Core.Entities;
using PieceOfCake.Shared.ViewModels.Product;

namespace PieceOfCake.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductDomainService _productDomainService;

        public ProductController(
            ILogger<ProductController> logger,
            IProductDomainService productDomainService
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productDomainService = productDomainService ?? throw new ArgumentNullException(nameof(productDomainService));
        }

        [HttpGet]
        public ActionResult<IReadOnlyCollection<Product>> Get()
        {
            var result = _productDomainService.Get();
            if (result.IsFailure)
                return Error<IReadOnlyCollection<Product>>(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductVm> Get(int id)
        {

            var result = _productDomainService.Get(id);
            if (result.IsFailure)
                return Error<ProductVm>(result.Error);

            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult<Product> Put(int id, [FromBody]string name)
        {
            var result = _productDomainService.Update(id, name);
            if (result.IsFailure)
                return Error<Product>(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public ActionResult<Product> Post([FromBody]string name)
        {
            var result = _productDomainService.Create(name);
            if (result.IsFailure)
                return Error<Product>(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var result = _productDomainService.Delete(id);
            if (result.IsFailure)
                return Error(result.Error);

            return Ok();
        }
    }
}
