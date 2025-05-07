using FinalProject.Core.Feature.Doctor.Command.Models;
using FinalProject.Core.Feature.Doctor.Query.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class DoctorController : Controller
    {
        private readonly IMediator _mediator;

        public DoctorController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _mediator.Send(new GetAllDoctorsQuery());
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var query = new GetDoctorByIdQuery(id);
            var response = await _mediator.Send(query);

            if (response == null)
            {
                TempData["ErrorMessage"] = "الدكتور غير موجود";
                return NotFound();
            }

            // تحويل GetDoctorByIdResponse إلى EditDoctorCommand
            var command = new EditDoctorCommand
            {
                Id = response.Id,
                Name = response.Name,
                //Departments = response.Departments,
                // DepatrmentId = response.DepartmentId,
                Phone = response.Phone,
                Email = response.Email,
                Image = response.Image,
                DoctorSchedules = response.DoctorSchedules,
                Gender = response.Gender,
                Details = response.Details,

                // ضيف باقي الخصائص هنا حسب الموديل عندك
            };

            return View(command); // View بتستقبل EditDoctorCommand
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddDoctorCommand command)
        {
            var doctorId = await _mediator.Send(command);
            if (doctorId == -1)
            {
                TempData["ErrorMessage"] = " الاسم موجود  بالفعل";
                return View();
            }

            TempData["SuccessMessage"] = "تم إضافة الطبيب بنجاح";
            return RedirectToAction("Index");

        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(EditDoctorCommand command)
        //{
        //    try
        //    {
        //        var doctorId = await _mediator.Send(command);

        //        if (doctorId == -1)
        //        {
        //            TempData["ErrorMessage"] = "الدكتور غير موجود";
        //            return View(command);
        //        }
        //        else if (doctorId == 0)
        //        {
        //            TempData["ErrorMessage2"] = "حدث خطأ أثناء التعديل";
        //            return View(command);
        //        }

        //        TempData["SuccessMessage"] = "تم تعديل الدكتور بنجاح";
        //        return RedirectToAction("Index");
        //    }
        //    catch (FluentValidation.ValidationException ex)
        //    {
        //        foreach (var error in ex.Errors)
        //        {
        //            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        //        }

        //        return View(command);
        //    }
        //}

    }
}
