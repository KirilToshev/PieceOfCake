using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PieceOfCake.Core.DomainServices.Interfaces;
using PieceOfCake.Core.Entities;
using PieceOfCake.Shared.ViewModels.MeasureUnit;

namespace PieceOfCake.Api.Controllers
{
    [ApiController]
    [Route("api/measureunits")]
    public class MeasureUnitController : Controller
    {
        private readonly ILogger<MeasureUnitController> _logger;
        private readonly IMeasureUnitDomainService _measureUnitDomainService;
        private readonly IMapper _mapper;

        public MeasureUnitController(
            ILogger<MeasureUnitController> logger,
            IMeasureUnitDomainService measureUnitDomainService,
            IMapper mapper
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _measureUnitDomainService = measureUnitDomainService ?? throw new ArgumentNullException(nameof(measureUnitDomainService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<MeasureUnitVm> Get()
        {
            var result = _measureUnitDomainService.Get();
            if (result.IsFailure)
                return BadRequest(result.Error);

            var mapping = result.Value.Select(x => _mapper.Map<MeasureUnitVm>(x));
            return Ok(mapping);
        }

        [HttpGet("{id}")]
        public ActionResult<MeasureUnitVm> Get(int id)
        {

            var result = _measureUnitDomainService.Get(id);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(_mapper.Map<MeasureUnitVm>(result.Value));
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
