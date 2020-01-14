using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using PieceOfCake.Api.Resources;
using PieceOfCake.Core.ValueObjects;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUserErrorsResource userErrors;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IStringLocalizer<UserErrorsResource> userErrorsLocalizer
            )
        {
            _logger = logger;
            userErrors = new UserErrorsResource(userErrorsLocalizer);
        }

        [HttpGet]
        public void Get()
        {
            var test = MeasureUnit.Create("", userErrors);
            var a = 1;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}
