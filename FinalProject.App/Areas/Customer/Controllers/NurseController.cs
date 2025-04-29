using FinalProject.Core.Feature.Nurse.Query.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class NurseController : Controller
    {
        private readonly IMediator _mediator;

        public NurseController( IMediator mediator  ) 
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> GetById([FromQuery] GetNurseByIdQuery query)
        {
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
