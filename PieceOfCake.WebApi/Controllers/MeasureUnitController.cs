using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PieceOfCake.Application.IngredientFeature.Dtos;
using PieceOfCake.Application.IngredientFeature.Services;
using PieceOfCake.DTOs.IngredientFeature;
using IResult = Microsoft.AspNetCore.Http.IResult;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PieceOfCake.WebApi.Controllers;
[Route("[controller]")]
[ApiController]
public class MeasureUnitController(
    IMapper mapper,
    IMeasureUnitService measureUnitService) : ControllerBase
{
    [HttpGet()]
    [ProducesResponseType<IEnumerable<MeasureUnitGetDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetAsync(CancellationToken cancellationToken)
    {
        var result = await measureUnitService.GetAllAsync(cancellationToken);
        return Results.Ok(mapper.Map<IEnumerable<MeasureUnitGetDto>>(result));
    }

    [HttpGet("{id}")]
    [ProducesResponseType<MeasureUnitGetDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await measureUnitService.GetByIdAsync(id, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<MeasureUnitGetDto>(p));   
    }

    [HttpPost]
    [ProducesResponseType<MeasureUnitGetDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Post([FromBody] MeasureUnitCreateDto createDto, CancellationToken cancellationToken)
    {
        var coreDto = mapper.Map<MeasureUnitCreateCoreDto>(createDto);
        var result = await measureUnitService.CreateAsync(coreDto, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<MeasureUnitGetDto>(p));
    }

    [HttpPut("{id}")]
    public async Task<IResult> Put(
        Guid id, 
        [FromBody] MeasureUnitUpdateDto updateDto, 
        CancellationToken cancellationToken)
    {
        var coreDto = mapper.Map<MeasureUnitUpdateCoreDto>(updateDto);
        var result = await measureUnitService.UpdateAsync(coreDto, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<MeasureUnitGetDto>(p));
    }

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await measureUnitService.DeleteAsync(id, cancellationToken);
        return result.ConvertToHttpResult();
    }
}
