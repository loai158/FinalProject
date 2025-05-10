using FinalProject.Core.Feature.Doctor.Command.Models;
using FinalProject.Core.Feature.Doctor.Query.Response;
using FinalProject.Data.Models.AppModels;

namespace FinalProject.Core.Mapping
{
    public static class DoctorMapProfile
    {
        public static GetDoctorByIdResponse MapDoctorResponse(this Doctor doctor)
        {
            return new GetDoctorByIdResponse
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Details = doctor.Details,
                Email = doctor.Email,
                Department = doctor.Department.Id,
                DoctorSchedules = doctor.DoctorSchedules,
                Gender = doctor.Gender,
                Phone = doctor.Phone,
                Image = doctor.Image,
            };

        }
        public static GetAllDoctorsResponse MapToDoctorRespone(this Doctor doctor)
        {
            return new GetAllDoctorsResponse
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Details = doctor.Details,
                Email = doctor.Email,
                Department = doctor.Department.Name,
                DoctorSchedules = doctor.DoctorSchedules,
                Gender = doctor.Gender,
                Phone = doctor.Phone,
                Image = doctor.Image,

            };
        }
        public static IEnumerable<GetAllDoctorsResponse> MapDoctorsResponseDTOs(this IEnumerable<Doctor> doctors)
        {
            return doctors.Select(c => c.MapToDoctorRespone());
        }
        public static Doctor MapAddToDoctor(this AddDoctorCommand command)
        {
            return new Doctor
            {
                Name = command.Name,
                DepartmentId = command.DepatrmentId,
                Details = command.Details,
                Email = command.Email,
                Image = command.Image,
                Phone = command.Phone,
                DoctorSchedules = command.DoctorSchedules,
                Gender = command.Gender,
            };
        }
        public static Doctor MapEditToDoctor(this EditDoctorCommand command)
        {
            return new Doctor
            {
                Id = command.Id, // أضف هذا السطر
                Name = command.Name,
                DepartmentId = command.Department,
                Details = command.Details,
                Email = command.Email,
                Image = command.Image,
                Phone = command.Phone,
                DoctorSchedules = command.DoctorSchedules,
                Gender = command.Gender,
            };
        }
    }
}
