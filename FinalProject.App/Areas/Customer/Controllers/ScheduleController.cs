using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ScheduleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDoctorServices _doctorServices;
        private readonly IScheduleServices _scheduleServices;

        public ScheduleController(UserManager<ApplicationUser> userManager, IDoctorServices doctorServices, IScheduleServices scheduleServices)
        {
            this._userManager = userManager;
            this._doctorServices = doctorServices;
            this._scheduleServices = scheduleServices;
        }
        public async Task<IActionResult> Index(string? dayFilter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var doctor = _doctorServices.GetAll().Where(d => d.IdentityUserId == userId).FirstOrDefault();
            if (doctor == null)
            {
                return NotFound("Doctor not found for current user.");
            }
            var response = await _scheduleServices.GetAllAsync(doctor.Id);
            if (!string.IsNullOrEmpty(dayFilter) && Enum.TryParse<DayOfWeek>(dayFilter, out var day))
            {
                response = response.Where(s => s.Day == day);
            }
            return View(response);
        }


        // POST: Schedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorSchedule schedule)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var doctor = _doctorServices.GetAll().FirstOrDefault(d => d.IdentityUserId == userId);
            if (doctor == null) return NotFound();

            schedule.DoctorId = doctor.Id;
            await _scheduleServices.AddAsync(schedule);
            return RedirectToAction(nameof(Index));


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorSchedule schedule)
        {
            var scheduleDb = await _scheduleServices.GetByIdAsync(schedule.Id);

            if (scheduleDb == null)
                throw new Exception("Schedule not found");


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var doctor = _doctorServices.GetAll().FirstOrDefault(d => d.IdentityUserId == userId);
            if (doctor == null) return NotFound();

            scheduleDb.DoctorId = doctor.Id;
            scheduleDb.StartTime = schedule.StartTime;
            scheduleDb.EndTime = schedule.EndTime;
            scheduleDb.Day = schedule.Day;
            await _scheduleServices.UpdateAsync(scheduleDb);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            await _scheduleServices.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
