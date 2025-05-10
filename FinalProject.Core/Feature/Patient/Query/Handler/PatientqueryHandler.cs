using FinalProject.Core.Feature.Patient.Query.Models;
using FinalProject.Core.Feature.Patient.Query.Response;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Patient.Query.Handler
{
    public class PatientqueryHandler : IRequestHandler<GetAllPatientQuery, IEnumerable<GetAllPatientResponse>>,
        IRequestHandler<GetPatientById, GetPatientByIdResponse>
    {
        private readonly IPatientServices _patientServices;

        public PatientqueryHandler(IPatientServices patientServices)
        {
            this._patientServices = patientServices;
        }
        public async Task<IEnumerable<GetAllPatientResponse>> Handle(GetAllPatientQuery request, CancellationToken cancellationToken)
        {
            var patients = _patientServices.GetAll();
            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var searchQuery = request.Query.ToLower();
                patients = patients
                    .Where(d => d.Name.ToLower().Contains(searchQuery) ||
                                d.Email.ToLower().Contains(searchQuery));
            }

            var totalCount = patients.ToList().Count();
            var pagedPatients = patients
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            var response = pagedPatients.MapPatientsToResponse();
            return response;
        }

        public async Task<GetPatientByIdResponse> Handle(GetPatientById request, CancellationToken cancellationToken)
        {
            //return the doctor then map
            var patient = await _patientServices.GetById(request.Id);
            var result = patient.PatientToResponse();
            return result;
        }
    }
}
