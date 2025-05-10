using FinalProject.Core.Feature.Patient.Command.Models;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Patient.Command.Handler
{
    public class PatientCommandHandler : IRequestHandler<AddNewPatient, int>,
        IRequestHandler<EditPatientCommand, bool>,
        IRequestHandler<DeletePatientCommand, string>
    {
        private readonly IPatientServices _patientServices;

        public PatientCommandHandler(IPatientServices patientServices)
        {
            this._patientServices = patientServices;
        }
        public Task<int> Handle(AddNewPatient request, CancellationToken cancellationToken)
        {
            //map First 

            var patient = request.MapToPatient();
            var result = _patientServices.Create(patient);
            return result;

        }

        public async Task<bool> Handle(EditPatientCommand request, CancellationToken cancellationToken)
        {
            //check if the Doctor  is exist first
            var doctor = await _patientServices.GetById(request.Id);
            if (doctor == null)
            {
                return false;
            }
            //map
            var result = request.MapEditToPatient();
            var final = _patientServices.Edit(result);
            if (final == "success")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await _patientServices.GetById(request.Id);
            if (patient.Image != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Patients", patient.Image);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }
            if (patient == null)
                return "patient not found";
            else
            {
                var result = _patientServices.Delete(request.Id);

                if (await result.ConfigureAwait(false) == "faild")

                    return "faild";

                else
                    return "success";
            }

        }
    }
}
