using FinalProject.Data.Models.AppModels;
using MediatR;

namespace FinalProject.Core.Feature.Patient.Command.Models
{
    public class AddNewPatient : IRequest<int>
    {
        public int DoctorId { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Image { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string IdentityUserId { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public ICollection<PreviousCondition>? PreviousConditions { get; set; }
        public ICollection<PreviousMedicine>? PreviousMedicines { get; set; }


    }
}
