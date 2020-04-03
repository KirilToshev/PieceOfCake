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

        protected new IActionResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }

        protected new IActionResult Ok(object result)
        {
            return base.Ok(Envelope.Ok(result));
        }
    }
}
