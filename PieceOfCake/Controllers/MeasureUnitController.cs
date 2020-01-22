using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;

namespace PieceOfCake.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeasureUnitController : ControllerBase
    {
        private readonly ILogger<MeasureUnitController> _logger;
        private readonly IResources _resources;
        private readonly IUnitOfWork _unitOfWork;

        public MeasureUnitController(
            ILogger<MeasureUnitController> logger,
            IResources resources,
            IUnitOfWork unitOfWork
            )
        {
            _logger = logger;
            _resources = resources;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public MeasureUnit Get(int id)
        {
            var test = MeasureUnit.Create("", _resources, _unitOfWork);
            return _unitOfWork.MeasureUnitRepository.GetById(id);
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
