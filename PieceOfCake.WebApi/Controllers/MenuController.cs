using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PieceOfCake.Application.MenuFeature.Dtos;
using PieceOfCake.Application.MenuFeature.Services;
using PieceOfCake.DTOs.MenuFeature;

namespace PieceOfCake.WebApi.Controllers;


[Route("[controller]")]
[ApiController]
public class MenuController(
    IMapper mapper,
    IMenuService menuService) : ControllerBase
{
    [HttpGet(Endpoints.Menus.GetAll)]
    [ProducesResponseType<IEnumerable<MenuGetDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetAsync(CancellationToken cancellationToken)
    {
        var result = await menuService.GetAllAsync(cancellationToken);
        return Results.Ok(mapper.Map<IEnumerable<MenuGetDto>>(result));
    }

    [HttpGet(Endpoints.Menus.Get)]
    [ProducesResponseType<MenuGetDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await menuService.GetByIdAsync(id, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<MenuGetDto>(p));
    }

    [HttpPost(Endpoints.Menus.Create)]
    [ProducesResponseType<MenuGetDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Post([FromBody] MenuCreateDto createDto, CancellationToken cancellationToken)
    {
        var coreDto = mapper.Map<MenuCreateCoreDto>(createDto);
        var result = await menuService.CreateAsync(coreDto, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<MenuGetDto>(p));
    }

    [HttpPut(Endpoints.Menus.Update)]
    [ProducesResponseType<MenuGetDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Put(
        Guid id,
        [FromBody] MenuUpdateDto updateDto,
        CancellationToken cancellationToken)
    {
        var coreDto = mapper.Map<MenuUpdateCoreDto>(updateDto);
        var result = await menuService.UpdateAsync(coreDto, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<MenuGetDto>(p));
    }

    [HttpDelete(Endpoints.Menus.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await menuService.DeleteAsync(id, cancellationToken);
        return result.ConvertToHttpResult();
    }

    [HttpPatch(Endpoints.Menus.GenerateDishesList)]
    [ProducesResponseType<MenuGetDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> GenerateDishesList(Guid id, CancellationToken cancellationToken)
    {
        var result = await menuService.GenerateDishesListAsync(id, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<MenuGetDto>(p));
    }
}
