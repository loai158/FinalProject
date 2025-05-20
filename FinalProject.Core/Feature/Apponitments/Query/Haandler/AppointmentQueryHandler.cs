using FinalProject.Core.Feature.Apponitments.Query.Models;
using FinalProject.Core.Feature.Apponitments.Query.Response;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Apponitments.Query.Haandler
{
    public class AppointmentQueryHandler : IRequestHandler<GetAllApponintmentsQuery, GetAllApponintmentsResponse>,
        IRequestHandler<GetAppointmentByIdQuery, GetAppointmentByIdResponse>,
        IRequestHandler<GetAllApponintmentsByDoctorIdQuery, GetAllApponintmentsResponse>

    {
        private readonly IAppointmentServices _appointmentServices;

        public AppointmentQueryHandler(IAppointmentServices appointmentServices)
        {
            this._appointmentServices = appointmentServices;
        }
        public async Task<GetAllApponintmentsResponse> Handle(GetAllApponintmentsQuery request, CancellationToken cancellationToken)
        {
            var appointments = _appointmentServices.GetAll(request.id);

            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var searchQuery = request.Query.ToLower();
                appointments = appointments
                    .Where(d => d.Patient.Name.ToLower().Contains(searchQuery) ||
                                d.Department.Name.ToLower().Contains(searchQuery));
            }

            var totalCount = appointments.Count(); // بدون ToList() لتوفير الأداء

            var pagedAppointments = appointments
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var mappedAppointments = pagedAppointments.MapAppointmentsToGetAll(); // Assuming this returns List<AppointmentDto>

            return new GetAllApponintmentsResponse
            {
                Appointments = mappedAppointments.Appointments,
                TotalCount = totalCount
            };
        }
        public async Task<GetAllApponintmentsResponse> Handle(GetAllApponintmentsByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            var appointments = _appointmentServices.GetAll().Where(d => d.DoctorId == request.doctorId);

            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var searchQuery = request.Query.ToLower();
                appointments = appointments
                    .Where(d => d.Patient.Name.ToLower().Contains(searchQuery) ||
                                d.Department.Name.ToLower().Contains(searchQuery));
            }

            var totalCount = appointments.Count(); // بدون ToList() لتوفير الأداء

            var pagedAppointments = appointments
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var mappedAppointments = pagedAppointments.MapAppointmentsToGetAll(); // Assuming this returns List<AppointmentDto>

            return new GetAllApponintmentsResponse
            {
                Appointments = mappedAppointments.Appointments,
                TotalCount = totalCount
            };

        }
        public async Task<GetAppointmentByIdResponse> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentServices.GetById(request.Id);
            var result = appointment.MapAppointmentToGetById();
            return result;
        }


    }
}
