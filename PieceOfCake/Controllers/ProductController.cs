using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductController(
            ILogger<ProductController> logger,
            IProductDomainService productDomainService,
            IMapper mapper
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productDomainService = productDomainService ?? throw new ArgumentNullException(nameof(productDomainService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IReadOnlyCollection<ProductVm>> Get()
        {
            var result = _productDomainService.Get();
            if (result.IsFailure)
                return Error<IReadOnlyCollection<ProductVm>>(result.Error);

            return Ok(result.Value.Select(x => _mapper.Map<ProductVm>(x)));
        }

        [HttpGet("{id}")]
        public ActionResult<ProductVm> Get(int id)
        {

            var result = _productDomainService.Get(id);
            if (result.IsFailure)
                return Error<ProductVm>(result.Error);

            return Ok(_mapper.Map<ProductVm>(result.Value));
        }

        [HttpPut("{id}")]
        public ActionResult<ProductVm> Put(int id, [FromBody]string name)
        {
            var result = _productDomainService.Update(id, name);
            if (result.IsFailure)
                return Error<ProductVm>(result.Error);

            return Ok(_mapper.Map<ProductVm>(result.Value));
        }

        [HttpPost]
        public ActionResult<ProductVm> Post([FromBody]ProductVm productVm)
        {
            var result = _productDomainService.Create(productVm.Name);
            if (result.IsFailure)
                return Error<ProductVm>(result.Error);

            return Ok(_mapper.Map<ProductVm>(result.Value));
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
