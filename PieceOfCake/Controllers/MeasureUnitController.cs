using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PieceOfCake.Core.BusinessRules;
using PieceOfCake.Core.Entities;

namespace PieceOfCake.Api.Controllers
{
    [ApiController]
    [Route("api/measureunits")]
    public class MeasureUnitController : Controller
    {
        private readonly ILogger<MeasureUnitController> _logger;
        private readonly IMeasureUnitBr _measureUnitsBr;

        public MeasureUnitController(
            ILogger<MeasureUnitController> logger,
            IMeasureUnitBr measureUnitsBr
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _measureUnitsBr = measureUnitsBr ?? throw new ArgumentNullException(nameof(measureUnitsBr));
        }

        [HttpGet]
        public ActionResult<IReadOnlyCollection<MeasureUnit>> Get()
        {
            var result = _measureUnitsBr.Get();
            if (result.IsFailure)
                return Error<IReadOnlyCollection<MeasureUnit>>(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public ActionResult<MeasureUnit> Get(int id)
        {

            var result = _measureUnitsBr.Get(id);
            if (result.IsFailure)
                return Error<MeasureUnit>(result.Error);

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public ActionResult<MeasureUnit> Put(int id, [FromBody]string name)
        {
            var result = _measureUnitsBr.Update(id, name);
            if (result.IsFailure)
                return Error<MeasureUnit>(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public ActionResult<MeasureUnit> Post([FromBody]string name)
        {
            var result = _measureUnitsBr.Create(name);
            if (result.IsFailure)
                return Error<MeasureUnit>(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _measureUnitsBr.Delete(id);
            if (result.IsFailure)
                return Error(result.Error);

            return Ok();
        }
    }
}
