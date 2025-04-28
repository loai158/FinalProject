using FinalProject.Core.Feature.Doctor.Query.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IMediator _mediator;

        public DoctorController( IMediator mediator )
        {
            this._mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _mediator.Send(new GetAllDoctorsQuery());
            return View(response);
        }
    }
}
