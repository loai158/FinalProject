using FinalProject.App.Helper.EmailSettings;
using FinalProject.Data.Models.SendEmailModel;
using FinalProject.Infrastructure.IRepositry;
using FinalProject.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DoctorApplyController : Controller
    {
        private readonly IRegisterApplyRepositoey _registerApply;
        private readonly IEmailSettings _emailSettings;
        private readonly IUnitOfWork _unitOfWork;

        public DoctorApplyController(IRegisterApplyRepositoey registerApply, 
            IEmailSettings emailSettings,IUnitOfWork unitOfWork)
        {
            _registerApply = registerApply;
            _emailSettings = emailSettings;
            this._unitOfWork = unitOfWork;
        }

        public IActionResult Index(string query, int page = 1)
        {
            var docApply = _registerApply.Get().Where(e => e.Role == 0);


            var paginationPages = (int)Math.Ceiling((decimal)docApply.Count() / 7);
            if (page > paginationPages) page = paginationPages;
            if (page < 1) page = 1;

            docApply = docApply.Skip((page - 1) * 7).Take(7);
            ViewBag.paginationPages = paginationPages;

            return View(docApply);
        }

        [HttpGet]
        public async Task<IActionResult> SendEmail(int id)
        {
            var user = await _registerApply.GetOne(e => e.Id == id);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Index");
            }

            var emailModel = new Email
            {
                To = user.Email
            };

            ViewBag.UserId = user.Id; // مهم للـ hidden input
            return View(emailModel);
        }

        [HttpPost]
        public IActionResult SendEmail(Email email, int userId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserId = userId; // نرجع الـ userId للـ View
                return View(email);
            }

            _emailSettings.SendEmail(email);
            TempData["Success"] = "Email sent successfully";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _registerApply.GetOne(e => e.Id == id);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Index");
            }
            _registerApply.Delete(user);
            _unitOfWork.RegisterApplyRepositoey.Commit();


            return RedirectToAction("Index");
        }



    }
}
