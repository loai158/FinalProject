using FinalProject.Core.Feature.Apponitments.Query.Response;
using FinalProject.Data.Models.AppModels;

namespace FinalProject.Core.Mapping
{
    public static class AppointmentMapProfile
    {
        public static GetAppointmentByIdResponse MapAppointmentToGetById(this Appointment appointment)
        {
            return new GetAppointmentByIdResponse
            {
                Id = appointment.Id,
                Date = appointment.Date,
                Doctor = appointment.Doctor.Name,
                Department = appointment.Department.Name,
                Patient = appointment.Patient.Name,
                Status = appointment.Status,
                Price = appointment.Price,
                Perscribtions = appointment.Perscribtions

            };
        }
        public static GetAllApponintmentsResponse MapAppointmentsToGetAll(this IEnumerable<Appointment> appointments)
        {
            return new GetAllApponintmentsResponse
            {
                Appointments = appointments.Select(c => c.MapAppointmentToGetById()).ToList()
            };
        }
    }
}
