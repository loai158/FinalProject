using FinalProject.Core.Feature.Apponitments.Query.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentController : Controller
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? query, int page = 1)
        {
            var respone = await _mediator.Send(new GetAllApponintmentsQuery
            {
                Query = query,
                Page = page,
                PageSize = 10
            });
            return View(respone);
        }
    }
}
