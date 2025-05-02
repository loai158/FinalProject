using FinalProject.Data.Models.AppModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;
using System.Numerics;

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

        public   IQueryable<Patient> GetAll()
        {
             var patients =  _unitOfWork.Repositry<Patient>().Get();
            return patients;
        }
    }
}
