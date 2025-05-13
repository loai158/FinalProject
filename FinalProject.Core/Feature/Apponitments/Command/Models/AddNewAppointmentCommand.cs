using FinalProject.Data.Models.AppModels;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Core.Feature.Apponitments.Command.Models
{
    public class AddNewAppointmentCommand : IRequest<int>
    {
        [Required(ErrorMessage = "field is required.")]
        public TypePayment TypePayment { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "DoctorId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "DoctorId must be a valid ID (greater than zero).")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "DepartmentId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "DepartmentId must be a valid ID (greater than zero).")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "PatientId is required.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(Status), ErrorMessage = "Status must be a valid value.")]
        public Status Status { get; set; }

    }
}