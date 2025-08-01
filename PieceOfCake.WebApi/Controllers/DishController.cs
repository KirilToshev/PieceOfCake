using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PieceOfCake.Application.DishFeature.Dtos;
using PieceOfCake.Application.DishFeature.Services;
using PieceOfCake.DTOs.DishFeature;
using IResult = Microsoft.AspNetCore.Http.IResult;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PieceOfCake.WebApi.Controllers;
[Route("[controller]")]
[ApiController]
public class DishController(
    IMapper mapper,
    IDishService dishService) : ControllerBase
{
    [HttpGet(Endpoints.Dishes.GetAll)]
    [ProducesResponseType<IEnumerable<DishDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetAsync(CancellationToken cancellationToken)
    {
        var result = await dishService.GetAllAsync(cancellationToken);
        return Results.Ok(mapper.Map<IEnumerable<DishDto>>(result));
    }

    [HttpGet(Endpoints.Dishes.Get)]
    [ProducesResponseType<DishDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Get([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var result = await dishService.GetByIdAsync(id, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<DishDto>(p));   
    }

    [HttpPost(Endpoints.Dishes.Create)]
    [ProducesResponseType<DishDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Post([FromBody] DishCreateDto createDto, CancellationToken cancellationToken)
    {
        var coreDto = mapper.Map<DishCreateCoreDto>(createDto);
        var result = await dishService.CreateAsync(coreDto, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<DishDto>(p));
    }

    [HttpPut("{id}")]
    public async Task<IResult> Put(
        Guid id, 
        [FromBody] DishUpdateDto updateDto, 
        CancellationToken cancellationToken)
    {
        var coreDto = mapper.Map<DishUpdateCoreDto>(updateDto);
        var result = await dishService.UpdateAsync(coreDto, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<DishDto>(p));
    }

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await dishService.DeleteAsync(id, cancellationToken);
        return result.ConvertToHttpResult();
    }
}
