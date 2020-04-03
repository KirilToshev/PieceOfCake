using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PieceOfCake.Core.BusinessRules;

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
        public IActionResult Get()
        {
            var result = _measureUnitsBr.Get();
            if (result.IsFailure)
                return Error(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            var result = _measureUnitsBr.Get(id);
            if (result.IsFailure)
                return Error(result.Error);

            return Ok(result.Value);
        }

        [HttpPut]
        public IActionResult Put(int id, string name)
        {
            var result = _measureUnitsBr.Update(id, name);
            if (result.IsFailure)
                return Error(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public IActionResult Post(string name)
        {
            var result = _measureUnitsBr.Create(name);
            if (result.IsFailure)
                return Error(result.Error);

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
