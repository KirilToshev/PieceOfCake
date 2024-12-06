using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PieceOfCake.Application.DishFeature.Dtos;
using PieceOfCake.Application.DishFeature.Services;
using PieceOfCake.DTOs.IngredientFeature;
using IResult = Microsoft.AspNetCore.Http.IResult;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PieceOfCake.WebApi.Controllers;
[Route("[controller]")]
[ApiController]
public class MealOfTheDayType(
    IMapper mapper,
    IMealOfTheDayTypeService mealOfTheDayTypeService) : ControllerBase
{
    [HttpGet()]
    [ProducesResponseType<IEnumerable<MealOfTheDayTypeGetDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetAsync(CancellationToken cancellationToken)
    {
        var result = await mealOfTheDayTypeService.GetAllAsync(cancellationToken);
        return Results.Ok(mapper.Map<IEnumerable<MealOfTheDayTypeGetDto>>(result));
    }

    [HttpGet("{id}")]
    [ProducesResponseType<MealOfTheDayTypeGetDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await mealOfTheDayTypeService.GetByIdAsync(id, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<MealOfTheDayTypeGetDto>(p));   
    }

    [HttpPost]
    [ProducesResponseType<MealOfTheDayTypeGetDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Post([FromBody] MealOfTheDayTypeCreateDto createDto, CancellationToken cancellationToken)
    {
        var coreDto = mapper.Map<MealOfTheDayTypeCreateCoreDto>(createDto);
        var result = await mealOfTheDayTypeService.CreateAsync(coreDto, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<MealOfTheDayTypeGetDto>(p));
    }

    [HttpPut("{id}")]
    public async Task<IResult> Put(
        Guid id, 
        [FromBody] MealOfTheDayTypeUpdateDto updateDto, 
        CancellationToken cancellationToken)
    {
        var coreDto = mapper.Map<MealOfTheDayTypeUpdateCoreDto>(updateDto);
        var result = await mealOfTheDayTypeService.UpdateAsync(coreDto, cancellationToken);
        return result.ConvertToHttpResult(p => mapper.Map<MealOfTheDayTypeGetDto>(p));
    }

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mealOfTheDayTypeService.DeleteAsync(id, cancellationToken);
        return result.ConvertToHttpResult();
    }
}
