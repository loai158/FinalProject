using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Data.Models.Medical;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AboutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPreviousConditionServices _previousCondition;
        private readonly IPreviousMedicineServices _previousMedicineServices;
        private readonly IPatientServices _patientServices;

        public AboutController(UserManager<ApplicationUser> userManager, IPreviousConditionServices previousCondition, IPreviousMedicineServices previousMedicineServices, IPatientServices _patientServices)
        {
            this._userManager = userManager;
            this._previousCondition = previousCondition;
            this._previousMedicineServices = previousMedicineServices;
            this._patientServices = _patientServices;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var patient = _patientServices.GetAll()
                .Include(x => x.PreviousConditions)
                .Include(x => x.PreviousMedicine)
                .FirstOrDefault(x => x.IdentityUserId == userId);

            if (patient == null)
            {
                return NotFound();
            }

            var model = new MedicalHistoryViM
            {
                PatientId = patient.Id,
                PatientName = patient.Name,
                PreviousConditions = patient.PreviousConditions ?? new List<PreviousCondition>(),
                PreviousMedicines = patient.PreviousMedicine ?? new List<PreviousMedicine>()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCondition(MedicalHistoryViM model)
        {
            if (model.NewConditionName.HasValue)
            {
                var newCondition = new PreviousCondition
                {
                    Name = model.NewConditionName.Value,
                    PatientId = model.PatientId
                };
                var result = await _previousCondition.Add(newCondition);
                if (result == "success")
                {
                    TempData["SuccessMessage"] = "تمت إضافة الحالة بنجاح.";
                }
                else
                {
                    TempData["ErrorMessage"] = "يرجى اختيار حالة صحيحة.";
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCondition(MedicalHistoryViM model)
        {
            if (!model.EditConditionId.HasValue || !model.EditConditionName.HasValue)
            {
                TempData["ErrorMessage"] = "يرجى إدخال بيانات صحيحة لتعديل الحالة.";
                return RedirectToAction("Index");
            }

            var condition = await _previousCondition.GetAll()
                .FirstOrDefaultAsync(c => c.Id == model.EditConditionId.Value);

            if (condition == null)
            {
                TempData["ErrorMessage"] = "الحالة غير موجودة.";
                return RedirectToAction("Index");
            }

            // التحقق من صلاحية المستخدم
            var patient = await _patientServices.GetAll()
                .FirstOrDefaultAsync(p => p.Id == condition.PatientId);
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (patient == null || patient.IdentityUserId != userId)
            {
                TempData["ErrorMessage"] = "غير مصرح لك بتعديل هذه الحالة.";
                return Unauthorized();
            }

            // تحديث الحالة
            condition.Name = model.EditConditionName.Value;

            var result = await _previousCondition.Update(condition);
            if (result == "success")
            {
                TempData["SuccessMessage"] = "تم تعديل الحالة بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تعديل الحالة. يرجى المحاولة مرة أخرى.";
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCondition(int id)
        {
            // جلب الحالة للتحقق من الصلاحية
            var condition = await _previousCondition.GetAll()
                .FirstOrDefaultAsync(c => c.Id == id);
            if (condition == null)
            {
                TempData["ErrorMessage"] = "الحالة غير موجودة.";
                return RedirectToAction("Index");
            }

            // التحقق من صلاحية المستخدم
            var patient = await _patientServices.GetAll()
                .FirstOrDefaultAsync(p => p.Id == condition.PatientId);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (patient == null || patient.IdentityUserId != userId)
            {
                TempData["ErrorMessage"] = "غير مصرح لك بحذف هذه الحالة.";
                return Unauthorized();
            }

            // تنفيذ الحذف
            var result = await _previousCondition.Delete(id);
            if (result == "success")
            {
                TempData["SuccessMessage"] = "تم حذف الحالة بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف الحالة.";
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMedicine(MedicalHistoryViM model)
        {
            if (string.IsNullOrEmpty(model.NewMedicineName))
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = $"بيانات النموذج غير صالحة: {string.Join("; ", errors)}";
                return RedirectToAction("Index");
            }

            // جلب المريض للتحقق من الصلاحية
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var patient = await _patientServices.GetAll()
                .FirstOrDefaultAsync(p => p.IdentityUserId == userId);

            if (patient == null)
            {
                TempData["ErrorMessage"] = "لم يتم العثور على سجل المريض.";
                return RedirectToAction("Index");
            }

            // إنشاء دواء جديد
            var medicine = new PreviousMedicine
            {
                Name = model.NewMedicineName,
                Dose = model.NewMedicineDose,
                StartDate = (DateTime)model.NewMedicineStartDate,
                EndDate = (DateTime)model.NewMedicineEndDate,
                PatientId = patient.Id
            };

            // إضافة الدواء
            var result = await _previousMedicineServices.Add(medicine);
            if (result == "success")
            {
                TempData["SuccessMessage"] = "تم إضافة الدواء بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة الدواء.";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedicine(MedicalHistoryViM model)
        {

            // التحقق من وجود البيانات المطلوبة
            if (!model.EditMedicineId.HasValue || string.IsNullOrEmpty(model.EditMedicineName) ||
                !model.EditMedicineStartDate.HasValue || !model.EditMedicineEndDate.HasValue)
            {
                TempData["ErrorMessage"] = "يرجى إدخال بيانات صحيحة لتعديل الدواء.";
                return RedirectToAction("Index");
            }

            // جلب الدواء من الداتابيز
            var medicine = await _previousMedicineServices.GetAll()
                .FirstOrDefaultAsync(m => m.Id == model.EditMedicineId.Value);

            if (medicine == null)
            {
                TempData["ErrorMessage"] = "الدواء غير موجود.";
                return RedirectToAction("Index");
            }

            // التحقق من صلاحية المستخدم
            var patient = await _patientServices.GetAll()
                .FirstOrDefaultAsync(p => p.Id == medicine.PatientId);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (patient == null || patient.IdentityUserId != userId)
            {
                TempData["ErrorMessage"] = "غير مصرح لك بتعديل هذا الدواء.";
                return Unauthorized();
            }

            // تحديث بيانات الدواء
            medicine.Name = model.EditMedicineName;
            medicine.Dose = model.EditMedicineDose;
            medicine.StartDate = model.EditMedicineStartDate.Value;
            medicine.EndDate = model.EditMedicineEndDate.Value;

            // تحديث الدواء في الداتابيز
            var result = await _previousMedicineServices.Update(medicine);
            if (result == "success")
            {
                TempData["SuccessMessage"] = "تم تعديل الدواء بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تعديل الدواء.";
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            // جلب الدواء للتحقق من الصلاحية
            var medicine = await _previousMedicineServices.GetAll()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicine == null)
            {
                TempData["ErrorMessage"] = "الدواء غير موجود.";
                return RedirectToAction("Index");
            }

            // التحقق من صلاحية المستخدم
            var patient = await _patientServices.GetAll()
                .FirstOrDefaultAsync(p => p.Id == medicine.PatientId);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (patient == null || patient.IdentityUserId != userId)
            {
                TempData["ErrorMessage"] = "غير مصرح لك بحذف هذا الدواء.";
                return Unauthorized();
            }

            // تنفيذ الحذف
            var result = await _previousMedicineServices.Delete(id);
            if (result == "success")
            {
                TempData["SuccessMessage"] = "تم حذف الدواء بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف الدواء.";
            }

            return RedirectToAction("Index");
        }
    }
}