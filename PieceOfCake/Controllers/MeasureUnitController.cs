using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PieceOfCake.Core.DomainServices.Interfaces;
using PieceOfCake.Core.Entities;
using PieceOfCake.Shared.ViewModels.MeasureUnit;

namespace PieceOfCake.Api.Controllers
{
    [ApiController]
    [Route("api/measureunits")]
    public class MeasureUnitController : ControllerBase
    {
        private readonly ILogger<MeasureUnitController> _logger;
        private readonly IMeasureUnitDomainService _measureUnitDomainService;

        public MeasureUnitController(
            ILogger<MeasureUnitController> logger,
            IMeasureUnitDomainService measureUnitDomainService
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _measureUnitDomainService = measureUnitDomainService ?? throw new ArgumentNullException(nameof(measureUnitDomainService));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _measureUnitDomainService.Get();
            if (result.IsFailure)
                return BadRequest(result.Error);

            //TODO: Introduce AutoMapper
            var mapping = result.Value.Select(x => new MeasureUnitVm()
            {
                Id = x.Id,
                Name = x.Name
            });

            return Ok(mapping);
        }

        [HttpGet("{id}")]
        public ActionResult<MeasureUnit> Get(int id)
        {

            var result = _measureUnitDomainService.Get(id);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public ActionResult<MeasureUnit> Put(int id, [FromBody]string name)
        {
            var result = _measureUnitDomainService.Update(id, name);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public ActionResult<MeasureUnit> Post([FromBody]string name)
        {
            var result = _measureUnitDomainService.Create(name);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var result = _measureUnitDomainService.Delete(id);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok();
        }
    }
}
