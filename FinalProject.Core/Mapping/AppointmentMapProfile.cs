using FinalProject.Core.Feature.Apponitments.Command.Models;
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
                DoctorId = appointment.Doctor.Id,
                DepartmentId = appointment.Department.Id,
                Department = appointment.Department.Name,
                PatientId = appointment.Patient.Id,
                Patient = appointment.Patient.Name,
                Status = appointment.Status,
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

        public static Appointment MapAddToAppointment(this AddNewAppointmentCommand command)
        {
            return new Appointment
            {
                Date = command.Date,
                DoctorId = command.DoctorId,
                PatientId = command.PatientId,
                Status = command.Status,
                DepartmentId = command.DepartmentId,
            };
        }
        public static Appointment MapEditToAppointment(this EditAppointmentCommand command)
        {
            return new Appointment
            {
                Id = command.Id,
                Date = command.Date,
                DoctorId = command.DoctorId,
                PatientId = command.PatientId,
                Status = command.Status,
                DepartmentId = command.DepartmentId,
                Perscribtions = command.Perscribtions,
            };
        }

    }
}
