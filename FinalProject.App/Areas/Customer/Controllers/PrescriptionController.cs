using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Data.Models.Medical;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PrescriptionController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPrescriptionService _prescriptionService;
        private readonly IPerscribtionMedicineService _perscribtionMedicineService;
        private readonly IDoctorServices _doctorServices;
        private readonly IAppointmentServices _appointmentServices;
        private readonly IMedicineServices _medicineServices;

        public PrescriptionController(UserManager<ApplicationUser> userManager, IPrescriptionService prescriptionService, IPerscribtionMedicineService perscribtionMedicineService, IDoctorServices doctorServices, IAppointmentServices appointmentServices, IMedicineServices medicineServices)
        {
            this._userManager = userManager;
            this._prescriptionService = prescriptionService;
            this._perscribtionMedicineService = perscribtionMedicineService;
            this._doctorServices = doctorServices;
            this._appointmentServices = appointmentServices;
            this._medicineServices = medicineServices;
        }
        [HttpGet]
        public IActionResult Index(int appointmentId)
        {
            var appointment = _appointmentServices.GetAll()
                                                 .Include(x => x.Doctor)
                                                 .Include(x => x.Patient)
                                                 .Include(x => x.Perscribtion)
                                                 .ThenInclude(p => p.PerscribtionMedicines)
                                                 .ThenInclude(pm => pm.Medicine)
                                                 .FirstOrDefault(x => x.Id == appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }
            var model = new PrescriptionVM
            {
                AppointmentId = appointmentId,
                DoctorName = appointment.Doctor.Name,
                PatientName = appointment.Patient.Name,
                PerscribtionMedicines = appointment.Perscribtion?.PerscribtionMedicines ?? new List<PerscribtionMedicine>()

            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMedicine(PrescriptionVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "الرجاء إدخال جميع الحقول المطلوبة.";
                return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
            }

            // التحقق من وجود الوصفة (Prescribtion)
            var prescribtion = await _prescriptionService
                .GetByAppointmentIdAsync((int)model.AppointmentId);



            if (prescribtion == null)
            {
                prescribtion = new Perscribtion
                {
                    AppointmentId = (int)model.AppointmentId,
                    PerscribtionMedicines = new List<PerscribtionMedicine>()
                };
                await _prescriptionService.Create(prescribtion);
            }

            // تحقق إذا كان الدواء موجود بالفعل
            var existingMedicine = await _medicineServices.GetByName(model.NewMedicineName);

            if (existingMedicine == null)
            {
                // إذا لم يكن موجود، أنشئه
                existingMedicine = new Medicine
                {
                    Name = model.NewMedicineName,
                    Dose = (int)model.NewMedicineDose, // يمكن تجاهله هنا لو حتخزنه فقط في PerscribtionMedicine
                    StartDate = (DateTime)model.NewMedicineStartDate,
                    EndDate = (DateTime)model.NewMedicineEndDate
                };

                await _medicineServices.Add(existingMedicine);
            }

            // أضف الدواء إلى جدول PerscribtionMedicine
            var perscribtionMedicine = new PerscribtionMedicine
            {
                PerscribtionId = prescribtion.Id,
                MedicineId = existingMedicine.Id,
                Dose = (int)model.NewMedicineDose,
                StartDate = (DateTime)model.NewMedicineStartDate,
                EndDate = (DateTime)model.NewMedicineEndDate
            };

            _perscribtionMedicineService.create(perscribtionMedicine);

            TempData["SuccessMessage"] = "تمت إضافة الدواء للوصفة بنجاح.";
            return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedicine(PrescriptionVM model)
        {
            // التأكد من وجود البيانات المطلوبة
            if (!model.EditMedicineId.HasValue || string.IsNullOrEmpty(model.EditMedicineName) ||
                !model.EditMedicineStartDate.HasValue || !model.EditMedicineEndDate.HasValue)
            {
                TempData["ErrorMessage"] = "يرجى إدخال بيانات صحيحة لتعديل الدواء.";
                return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
            }

            // جلب المستخدم الحالي
            var user = await _userManager.GetUserAsync(User);

            // التحقق من صلاحية الدكتور
            var doctor = await _doctorServices.GetAll()
                .FirstOrDefaultAsync(p => p.IdentityUserId == user.Id);

            if (doctor == null)
            {
                TempData["ErrorMessage"] = "غير مصرح لك بتعديل هذا الدواء.";
                return Unauthorized();
            }

            // البحث عن سجل PerscribtionMedicine بناءً على EditMedicineId
            var prescribtionMedicine = await _perscribtionMedicineService.GetAll()
                .Include(pm => pm.Medicine)
                .FirstOrDefaultAsync(pm => pm.Id == model.EditMedicineId.Value);

            if (prescribtionMedicine == null)
            {
                TempData["ErrorMessage"] = "لم يتم العثور على هذا الدواء في الروشتة.";
                return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
            }

            // تحديث البيانات داخل PerscribtionMedicine
            prescribtionMedicine.Medicine.Name = model.EditMedicineName;
            prescribtionMedicine.StartDate = model.EditMedicineStartDate.Value;
            prescribtionMedicine.EndDate = model.EditMedicineEndDate.Value;

            if (int.TryParse(model.EditMedicineDose, out int dose))
            {
                prescribtionMedicine.Dose = dose;
            }
            else
            {
                TempData["ErrorMessage"] = "الجرعة غير صالحة.";
                return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
            }

            // حفظ التغييرات
            var result = _perscribtionMedicineService.Edit(prescribtionMedicine);

            if (result == "success")
            {
                TempData["SuccessMessage"] = "تم تعديل الدواء بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تعديل الدواء.";
            }

            return RedirectToAction("Index", new { appointmentId = model.AppointmentId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedicine(int id, int appointmentId)
        {
            // جلب المستخدم الحالي
            var user = await _userManager.GetUserAsync(User);

            // التحقق من صلاحية الدكتور
            var doctor = await _doctorServices.GetAll()
                .FirstOrDefaultAsync(p => p.IdentityUserId == user.Id);

            if (doctor == null)
            {
                TempData["ErrorMessage"] = "غير مصرح لك بحذف هذا الدواء.";
                return Unauthorized();
            }

            // جلب علاقة الدواء بالروشتة
            var prescribtionMedicine = await _perscribtionMedicineService.GetAll()
                .FirstOrDefaultAsync(pm => pm.Id == id);

            if (prescribtionMedicine == null)
            {
                TempData["ErrorMessage"] = "لم يتم العثور على هذا الدواء في الروشتة.";
                return RedirectToAction("Index", new { appointmentId });
            }

            // حذف العلاقة
            var result = await _perscribtionMedicineService.Delete(prescribtionMedicine.Id);

            if (result == "success")
            {
                TempData["SuccessMessage"] = "تم حذف الدواء من الروشتة بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حذف الدواء.";
            }

            return RedirectToAction("Index", new { appointmentId });
        }


    }
}
