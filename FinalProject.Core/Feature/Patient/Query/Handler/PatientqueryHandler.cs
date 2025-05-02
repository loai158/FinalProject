using FinalProject.Core.Feature.Patient.Query.Models;
using FinalProject.Core.Feature.Patient.Query.Response;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Patient.Query.Handler
{
    public class PatientqueryHandler : IRequestHandler<GetAllPatientQuery, IEnumerable<GetAllPatientResponse>>
    {
        private readonly IPatientServices _patientServices;

        public PatientqueryHandler(IPatientServices patientServices)
        {
            this._patientServices = patientServices;
        }
        public async Task<IEnumerable<GetAllPatientResponse>> Handle(GetAllPatientQuery request, CancellationToken cancellationToken)
        {
            //retrive the patients then map 
            var patients = _patientServices.GetAll();
            var result =  patients.MapPatientsToResponse().AsEnumerable();
            return result;
        }
    }
}
