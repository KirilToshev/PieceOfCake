using Microsoft.AspNetCore.Mvc;
using PieceOfCake.DTOs.IngredientFeature;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PieceOfCake.WebApi.Controllers;
[Route("[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    // GET: <ProductController>
    [HttpGet]
    [ProducesResponseType<IEnumerable<ProductGetDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public string[] Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET <ProductController>/5
    [HttpGet("{id}")]
    [ProducesResponseType<ProductGetDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public string Get(int id)
    {
        return "value";
    }

    // POST <ProductController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT <ProductController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE <ProductController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
