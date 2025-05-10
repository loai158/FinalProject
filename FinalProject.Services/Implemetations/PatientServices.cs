using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

namespace FinalProject.Services.Implemetations
{
    public class PatientServices : IPatientServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<int> Create(Patient patient)
        {
            var result = await _unitOfWork.Repositry<Patient>().Exist(p => p.Name == patient.Name);
            if (result)
            {
                //name exist
                return -1;
            }

            var final = await _unitOfWork.Repositry<Patient>().Create(patient);
            await _unitOfWork.CompleteAsync();
            if (final == "success")
            {
                return patient.Id;
            }
            else
                return 0;
        }
        public async Task<string> Delete(int id)
        {
            var trans = _unitOfWork.BeginTransactionAsync();
            try
            {
                var patient = await _unitOfWork.Repositry<Patient>().GetOne(d => d.Id == id);
                _unitOfWork.Repositry<Patient>().Delete(patient);
                _unitOfWork.Repositry<Patient>().Commit();
                await _unitOfWork.CommitTransactionAsync();
                return "success";
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return "faild";

            }
        }
        public string Edit(Patient patient)
        {
            _unitOfWork.Repositry<Patient>().Edit(patient);
            _unitOfWork.Repositry<Patient>().Commit();
            return "success";
        }

        public IQueryable<Patient> GetAll()
        {
            var patients = _unitOfWork.Repositry<Patient>().Get(includes: [a => a.Appointments, b => b.PreviousConditions, c => c.PreviousMedicine]);
            return patients;
        }

        public async Task<Patient> GetById(int id)
        {
            var patient = await _unitOfWork.Repositry<Patient>()
                .GetOne(c => c.Id == id, includes: [a => a.Appointments,
                                                    b => b.PreviousConditions,
                                                    c => c.PreviousMedicine],
                                                    tracked: false
                                                    );
            return patient;
        }
    }
}
