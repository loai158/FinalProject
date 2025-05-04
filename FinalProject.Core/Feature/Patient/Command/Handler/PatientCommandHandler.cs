using FinalProject.Core.Feature.Patient.Command.Models;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Patient.Command.Handler
{
    public class PatientCommandHandler : IRequestHandler<AddNewPatient, int>
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
    }
}
