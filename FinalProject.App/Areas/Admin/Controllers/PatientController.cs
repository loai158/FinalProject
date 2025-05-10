using FinalProject.Core.Feature.Patient.Command.Models;
using FinalProject.Core.Feature.Patient.Query.Models;
using FinalProject.Services.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PatientController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IDepartmentServices _departmentServices;
        private readonly IDoctorServices _doctorServices;

        public PatientController(IMediator mediator, IDepartmentServices departmentServices, IDoctorServices doctorServices)
        {

            _mediator = mediator;
            this._departmentServices = departmentServices;
            this._doctorServices = doctorServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? query, int page = 1)
        {
            var response = await _mediator.Send(new GetAllPatientQuery
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
            ViewData["Previous"] = _departmentServices.getAll().ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddNewPatient command, IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Patients", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                command.Image = fileName;
            }

            try
            {
                var PatientId = await _mediator.Send(command);

                if (PatientId == -1)
                {
                    TempData["Error"] = "الاسم موجود بالفعل";
                    return View(command);
                }

                TempData["Success"] = "تم إضافة المريض بنجاح";
                return RedirectToAction("Index");
            }
            catch (FluentValidation.ValidationException ex)
            {
                // أضف الأخطاء إلى ModelState
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(command); // هذا يعرض الأخطاء في الـ View
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int patientId)
        {
            var query = new GetPatientById(patientId);
            var response = await _mediator.Send(query);
            if (response != null)
            {
                return View(response);
            }
            return RedirectToAction("Error", "Home", new { area = "Customer" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPatientCommand model, IFormFile? file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Patients", fileName);
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
        public async Task<IActionResult> Delete(int patientId)
        {
            var response = await _mediator.Send(new DeletePatientCommand(patientId));
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
