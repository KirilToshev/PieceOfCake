using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PieceOfCake.Core.DomainServices.Interfaces;
using PieceOfCake.Core.Resources;
using AutoMapper;
using PieceOfCake.Shared.ViewModels.Dish;
using PieceOfCake.Shared.ViewModels.Menu;
using PieceOfCake.Core.Persistence;

namespace PieceOfCake.Api.Controllers
{
    [ApiController]
    [Route("api/menu")]
    public class MenuController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IResources _resources;
        private readonly IMapper _mapper;

        private readonly IMenuDomainService _menuDomainService;
        private readonly IUnitOfWork _unitOfWork;

        public MenuController(
            ILogger<ProductController> logger,
            IResources resources,
            IMenuDomainService dishDomainService,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _menuDomainService = dishDomainService ?? throw new ArgumentNullException(nameof(dishDomainService));
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IReadOnlyCollection<MenuVm>> Get()
        {
            var result = _menuDomainService.Get();
            if (result.IsFailure)
                return Error<IReadOnlyCollection<MenuVm>>(result.Error);

            return Ok(result.Value.Select(x => _mapper.Map<MenuVm>(x)));
        }

        [HttpGet("{id}")]
        public ActionResult<MenuVm> Get(int id)
        {

            var result = _menuDomainService.Get(id);
            if (result.IsFailure)
                return Error<MenuVm>(result.Error);

            return Ok(_mapper.Map<MenuVm>(result.Value));
        }

        [HttpPut("{id}")]
        public ActionResult<MenuVm> Put(int id, [FromBody]MenuVm menuVm)
        {
            var result = _menuDomainService.Update(id, menuVm.StartDate, menuVm.EndDate, menuVm.ServingsPerDay);
            if (result.IsFailure)
                return Error<MenuVm>(result.Error);

            return Ok(_mapper.Map<MenuVm>(result.Value));
        }

        [HttpPost]
        public ActionResult<MenuVm> Post([FromBody]MenuVm menuVm)
        {
            var result = _menuDomainService.Create(menuVm.StartDate, menuVm.EndDate, menuVm.ServingsPerDay);
            if (result.IsFailure)
                return Error<MenuVm>(result.Error);

            return Ok(_mapper.Map<MenuVm>(result.Value));
        }

        [HttpPatch("{id}")]
        public ActionResult<MenuVm> GenerateDishesList(long id)
        {
            var result = _menuDomainService.GenerateDishesList(id);
            if (result.IsFailure)
                return Error<MenuVm>(result.Error);

            return Ok(_mapper.Map<MenuVm>(result.Value));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var result = _menuDomainService.Delete(id);
            if (result.IsFailure)
                return Error(result.Error);

            return Ok();
        }
    }
}
