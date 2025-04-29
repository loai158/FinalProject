using FinalProject.Core.Feature.Doctor.Query.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class DoctorController : Controller
    {
        private readonly IMediator _mediator;

        public DoctorController( IMediator mediator )
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _mediator.Send(new GetAllDoctorsQuery());
            return View(response);
        }
    }
}
