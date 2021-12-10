using Microsoft.AspNetCore.Mvc;

namespace DotNET.DDNS.Worker.Controllers
{
    [Route("Logs")]
    public class LogsController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new ViewResult();
        }
    }
}
