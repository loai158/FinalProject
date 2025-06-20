using FinalProject.Core.Feature.Doctor.Command.Models;
using FinalProject.Core.Feature.Doctor.Query.Models;
using FinalProject.Services.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class DoctorController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IDepartmentServices _departmentServices;
        private readonly IDoctorServices _doctorServices;

        public DoctorController(IMediator mediator, IDepartmentServices departmentServices, IDoctorServices doctorServices)
        {

            _mediator = mediator;
            this._departmentServices = departmentServices;
            this._doctorServices = doctorServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? query, int page = 1)
        {
            var response = await _mediator.Send(new GetAllDoctorsQuery
            {
                Query = query,
                Page = page,
                PageSize = 10
            });
            ViewBag.CurrentQuery = query;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(response.Count() / 10.0); // عدد الصفحات الكلي
            return View(response);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Departments"] = _departmentServices.getAll().ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddDoctorCommand command, IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Doctors", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                // Save img name in db
                command.Image = fileName;
            }
            var doctorId = await _mediator.Send(command);
            if (doctorId == -1)
            {
                TempData["ErrorMessage"] = " الاسم موجود  بالفعل";
                ViewData["Departments"] = _departmentServices.getAll().ToList();
                return View();
            }

            TempData["SuccessMessage"] = "تم إضافة الطبيب بنجاح";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int doctorId)
        {
            var query = new GetDoctorByIdQuery(doctorId);
            var response = await _mediator.Send(query);
            if (response != null)
            {
                ViewBag.Departments = _departmentServices.getAll()
               .Select(d => new SelectListItem
               {
                   Value = d.Id.ToString(),
                   Text = d.Name
               })
                .ToList();
                return View(response);
            }
            return RedirectToAction("Error", "Home", new { area = "Customer" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditDoctorCommand model, IFormFile? file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Doctors", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    model.Image = fileName;
                }

                var response = await _mediator.Send(model);

                if (response)
                {
                    return RedirectToAction("Index");
                }


                return View(model);
            }
            catch (FluentValidation.ValidationException ex)
            {

                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }


                ViewBag.Departments = _departmentServices.getAll()
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                 .ToList();

                return View(model);
            }

        }

        public async Task<IActionResult> Delete(int doctorId)
        {
            var response = await _mediator.Send(new DeleteDoctorCommand(doctorId));
            if (response == "success")
            {
                TempData["Success"] = "تم الحذف بنجاح";
                return RedirectToAction("index");
            }
            else
            {
                TempData["Error"] = "يرجي حذف المواعيد اولا";
                return RedirectToAction("index");
            }
        }
    }

}

