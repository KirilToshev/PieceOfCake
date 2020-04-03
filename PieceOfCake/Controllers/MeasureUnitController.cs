using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using PieceOfCake.Api.Resources;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Extensions;

namespace PieceOfCake.Api.Controllers
{
    [ApiController]
    [Route("api/measureunits")]
    public class MeasureUnitController : Controller
    {
        private readonly ILogger<MeasureUnitController> _logger;
        private readonly IResources _resources;
        private readonly IUnitOfWork _unitOfWork;

        public MeasureUnitController(
            ILogger<MeasureUnitController> logger,
            IResources resources,
            IUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var measureUnitsList = _unitOfWork.MeasureUnitRepository.Get();
            return Ok(measureUnitsList);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var measureUnit = _unitOfWork.MeasureUnitRepository.GetById(id);
            if (measureUnit == null)
            {
                return Error("Move this to a separate layer");
            }

            return Ok(measureUnit);
        }

        [HttpPut]
        public void Put(int id, string name)
        {
            var measureUnit = _unitOfWork.MeasureUnitRepository.GetById(id);
            
        }

        [HttpPost]
        public void Post(string name)
        {
            var test = MeasureUnit.Create("", _resources, _unitOfWork);
            var a = 1;
        }

        [HttpDelete]
        public void Delete(int id)
        {
            var test = MeasureUnit.Create("", _resources, _unitOfWork);
            var a = 1;
        }
    }
}
