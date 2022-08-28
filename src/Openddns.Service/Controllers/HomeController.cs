using MediatR;
using Microsoft.AspNetCore.Mvc;
using Openddns.Application.Queries;

namespace Openddns.Service.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("~/")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _mediator.Send(new GetLogsQuery(), cancellationToken));
        }
    }
}