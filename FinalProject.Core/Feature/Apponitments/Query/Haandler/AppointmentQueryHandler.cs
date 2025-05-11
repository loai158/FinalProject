using FinalProject.Core.Feature.Apponitments.Query.Models;
using FinalProject.Core.Feature.Apponitments.Query.Response;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Apponitments.Query.Haandler
{
    public class AppointmentQueryHandler : IRequestHandler<GetAllApponintmentsQuery, GetAllApponintmentsResponse>
    {
        private readonly IAppointmentServices _appointmentServices;

        public AppointmentQueryHandler(IAppointmentServices appointmentServices)
        {
            this._appointmentServices = appointmentServices;
        }
        public async Task<GetAllApponintmentsResponse> Handle(GetAllApponintmentsQuery request, CancellationToken cancellationToken)
        {
            var appointments = _appointmentServices.GetAll();
            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var searchQuery = request.Query.ToLower();
                appointments = appointments
                    .Where(d => d.Patient.Name.ToLower().Contains(searchQuery) ||
                                d.Department.Name.ToLower().Contains(searchQuery));
            }

            var totalCount = appointments.ToList().Count();
            var pagedAppointments = appointments
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            var response = pagedAppointments.MapAppointmentsToGetAll();
            return response;
        }
    }
}
