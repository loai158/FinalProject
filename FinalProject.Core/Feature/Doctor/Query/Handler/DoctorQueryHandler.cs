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
            //retrn the doctor the map
            var doctor = await _doctorServices.GetById(request.Id);
            var result = doctor.MapDoctorResponse();
            var depts = _departmentServices.getAll().ToList();
            var final = new GetDoctorByIdResponse
            {
                Id = result.Id,
                Name = result.Name,
                Details = result.Details,
                Phone = result.Phone,
                Image = result.Image,
                Email = result.Email,
                Gender = result.Gender,
                DepartmentId = result.DepartmentId,
                Departments = depts,
            };
            return final;
        }
    }
}
