using FinalProject.Core.Feature.Doctor.Query.Models;
using FinalProject.Core.Feature.Doctor.Query.Response;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Doctor.Query.Handler
{
    public class DoctorQueryHandler : IRequestHandler<GetAllDoctorsQuery, IEnumerable<GetAllDoctorsResponse>>
    {
        private readonly IDoctorServices _doctorServices;

        public DoctorQueryHandler(IDoctorServices doctorServices )
        {
            this._doctorServices = doctorServices;
        }
        public async Task<IEnumerable<GetAllDoctorsResponse>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            var doctors = _doctorServices.GetAll();



            var response = doctors.MapDoctorsResponseDTOs();
            return response;
        }
    }
}
