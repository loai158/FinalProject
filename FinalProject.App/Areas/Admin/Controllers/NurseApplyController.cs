using FinalProject.App.Helper.EmailSettings;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Data.Models.SendEmailModel;
using FinalProject.Infrastructure.IRepositry;
using FinalProject.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinalProject.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NurseApplyController : Controller
    {
        private readonly IRegisterApplyRepositoey _registerApply;
        private readonly IEmailSettings _emailSettings;
        private readonly IUnitOfWork _unitOfWork;

        public NurseApplyController(IRegisterApplyRepositoey registerApply, IEmailSettings emailSettings
            ,IUnitOfWork unitOfWork)
        {
            this._registerApply = registerApply;
            this._emailSettings = emailSettings;
            this._unitOfWork = unitOfWork;
        }
        public  IActionResult Index(string query, int page = 1)
        {
            var nursApply = _registerApply.Get().Where(e => e.Role == RoleType.Nurse);


            //filter
            if (query != null)
            {
                nursApply = nursApply.Where(e => e.FullName.Contains(query)
                || e.Email.Contains(query));
            }
            //pagination
            var paginationPages = (int)Math.Ceiling((decimal)nursApply.Count() / 7);
            if (page > paginationPages) page = paginationPages;
            nursApply = nursApply.Skip((page - 1) * 7).Take(7);
            ViewBag.paginationPages = paginationPages;
            return View(nursApply);
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
                To = user.Email // نملأ الـ email تلقائي
            };

            ViewBag.UserId = user.Id;
            return View(emailModel);
        }

        [HttpPost]
        public IActionResult SendEmail(Email email, int userId)
        {
            if (!ModelState.IsValid)
            {
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
