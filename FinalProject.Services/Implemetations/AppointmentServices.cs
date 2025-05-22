using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services.Implemetations
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentServices(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }
        public IQueryable<Appointment> GetAll(string userId)
        {
            var Appointments = _unitOfWork.Repositry<Appointment>().Get(
                includes: [
                    d => d.Perscribtion,
                    s => s.Schedule,
                    x => x.Department,
                    z => z.Patient,
                    p => p.Doctor]).Where(u => u.Patient.IdentityUserId == userId);
            return Appointments;
        }
        public async Task<int> Create(Appointment appointment)
        {
            var final = await _unitOfWork.Repositry<Appointment>().Create(appointment);
            await _unitOfWork.CompleteAsync();
            if (final == "success")
            {
                return appointment.Id;
            }
            else
                return 0;
        }
        public async Task<Appointment> GetById(int id)
        {
            var appointment = await _unitOfWork.Repositry<Appointment>().GetOne(n => n.Id == id, tracked: false, includes: [d => d.Department, W => W.Patient, x => x.Doctor, Q => Q.Perscribtion]);
            return appointment;
        }
        public string Edit(Appointment appointment)
        {
            _unitOfWork.Repositry<Appointment>().Edit(appointment);
            _unitOfWork.Repositry<Appointment>().Commit();
            return "success";
        }
        public async Task<string> Delete(int id)
        {
            var trans = _unitOfWork.BeginTransactionAsync();
            try
            {
                var appointment = await _unitOfWork.Repositry<Appointment>().GetOne(d => d.Id == id);
                _unitOfWork.Repositry<Appointment>().Delete(appointment);
                _unitOfWork.Repositry<Appointment>().Commit();
                await _unitOfWork.CommitTransactionAsync();
                return "success";
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return "faild";
            }
        }
        public async Task<int?> GetPatientIdFromUserAsync(string userId)
        {
            var patient = await _unitOfWork.Repositry<Patient>().Get()
              .Where(p => p.IdentityUserId == userId)
              .Select(p => p.Id)
              .FirstOrDefaultAsync();

            return patient == 0 ? null : patient;
        }
        public async Task<int?> GetDoctorIdFromUserAsync(string userId)
        {
            var patient = await _unitOfWork.Repositry<Doctor>().Get()
              .Where(p => p.IdentityUserId == userId)
              .Select(p => p.Id)
              .FirstOrDefaultAsync();

            return patient == 0 ? null : patient;
        }

        public IQueryable<Appointment> GetAll()
        {
            var Appointments = _unitOfWork.Repositry<Appointment>().Get(
                 includes: [
                     d => d.Perscribtion,
                    s => s.Schedule,
                    x => x.Department,
                    z => z.Patient,
                    p => p.Doctor]);
            return Appointments;
        }
    }
}
