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
public class ProductController(
    IMapper mapper,
    IProductService productService) : ControllerBase
{
    // GET: <ProductController>
    [HttpGet(Name = "Get")]
    [ProducesResponseType<IEnumerable<ProductGetDto>>(StatusCodes.Status200OK)]
    public async Task<IResult> GetAsync(CancellationToken cancellationToken)
    {
        var products = await productService.GetAllAsync(cancellationToken);
        return Results.Ok(mapper.Map<IEnumerable<ProductGetDto>>(products));
    }

    // GET <ProductController>/5
    [HttpGet("{id}")]
    [ProducesResponseType<ProductGetDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdAsync(id, cancellationToken);
        return product.ConvertToHttpResult(p => mapper.Map<ProductGetDto>(p));   
    }

    // POST <ProductController>
    [HttpPost]
    [ProducesResponseType<ProductGetDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Post([FromBody] ProductCreateDto product, CancellationToken cancellationToken)
    {
        var coreDto = mapper.Map<ProductCreateCoreDto>(product);
        var productResult = await productService.CreateAsync(coreDto, cancellationToken);
        return productResult.ConvertToHttpResult(p => mapper.Map<ProductGetDto>(p));
    }

    // PUT <ProductController>/5
    [HttpPut("{id}")]
    public async Task<IResult> Put(
        Guid id, 
        [FromBody] ProductUpdateDto product, 
        CancellationToken cancellationToken)
    {
        var coreDto = mapper.Map<ProductUpdateCoreDto>(product);
        var productResult = await productService.UpdateAsync(coreDto, cancellationToken);
        return productResult.ConvertToHttpResult(p => mapper.Map<ProductGetDto>(p));
    }

    // DELETE <ProductController>/5
    [HttpDelete("{id}")]
    public async Task<IResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await productService.DeleteAsync(id, cancellationToken);
        return result.ConvertToHttpResult();
    }
}
