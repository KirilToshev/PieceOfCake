using Microsoft.AspNetCore.Mvc;
using PieceOfCake.Api.Models;

namespace PieceOfCake.Api.Controllers
{
    public class Controller : ControllerBase
    {
        public Controller()
        {
        }

        protected IActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }

        protected ActionResult<T> Error<T>(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }

        public new OkObjectResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }

        public override OkObjectResult Ok(object result)
        {
            return base.Ok(Envelope.Ok(result));
        }
    }
}
