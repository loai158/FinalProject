using FinalProject.Core.Feature.Nurse.Command.Models;
using FinalProject.Core.Feature.Nurse.Query.Models;
using FinalProject.Services.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NurseController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IDepartmentServices _departmentServices;

        public NurseController(IMediator mediator, IDepartmentServices departmentServices)
        {
            this._mediator = mediator;
            this._departmentServices = departmentServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? query, int page = 1)
        {
            var response = await _mediator.Send(new GetAllNursesQuery
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
        public async Task<ActionResult> Create(AddNewNurseCommand command, IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Nurses", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                // Save img name in db
                command.Image = fileName;
            }
            var nurseId = await _mediator.Send(command);
            if (nurseId == -1)
            {
                TempData["Error"] = " الاسم موجود  بالفعل";
                return View();
            }

            TempData["Success"] = "تم إضافة الممريض بنجاح";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int nurseId)
        {
            var query = new GetNurseByIdQuery(nurseId);
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
        public async Task<IActionResult> Edit(EditNurseCommand model, IFormFile? file)
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

            ViewBag.Departments = _departmentServices.getAll()
           .Select(d => new SelectListItem
           {
               Value = d.Id.ToString(),
               Text = d.Name
           })
            .ToList();
            return View(model);
        }



        public async Task<IActionResult> Delete(int nurseId)
        {
            var response = await _mediator.Send(new DeleteNurseCommand(nurseId));
            if (response == "success")
            {
                TempData["Success"] = "تم الحذف بنجاح";
                return RedirectToAction("index");
            }
            else
            {
                TempData["Error"] = "يرجي حذف الريكورات اولا";
                return RedirectToAction("index");
            }
        }
    }
}
