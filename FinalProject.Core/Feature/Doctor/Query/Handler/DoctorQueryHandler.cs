using FinalProject.Core.Feature.Doctor.Query.Models;
using FinalProject.Core.Feature.Doctor.Query.Response;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Doctor.Query.Handler
{
    public class DoctorQueryHandler : IRequestHandler<GetAllDoctorsQuery, IEnumerable<GetAllDoctorsResponse>>,
        IRequestHandler<GetDoctorByIdQuery, GetDoctorByIdResponse>
    {
        private readonly IDoctorServices _doctorServices;
        private readonly IDepartmentServices _departmentServices;

        public DoctorQueryHandler(IDoctorServices doctorServices, IDepartmentServices departmentServices)
        {
            this._doctorServices = doctorServices;
            this._departmentServices = departmentServices;
        }
        public async Task<IEnumerable<GetAllDoctorsResponse>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            var doctors = _doctorServices.GetAll();
            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var searchQuery = request.Query.ToLower();
                doctors = doctors
                    .Where(d => d.Name.ToLower().Contains(searchQuery) ||
                                d.Email.ToLower().Contains(searchQuery));
            }

            var totalCount = doctors.ToList().Count();
            var pagedDoctors = doctors
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            var response = pagedDoctors.MapDoctorsResponseDTOs();
            return response;
        }

        public async Task<GetDoctorByIdResponse> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            //return the doctor then map
            var doctor = await _doctorServices.GetById(request.Id);
            var result = doctor.MapDoctorResponse();
            return result;
        }
    }
}
