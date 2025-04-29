using FinalProject.Data.Models.AppModels;
using MediatR;

namespace FinalProject.Core.Feature.Doctor.Command.Models
{
    public class AddDoctorCommand:IRequest<int>
    {
        public string Name { get; set; }
        public string Details { get; set; }

        public string Phone { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public int DepatrmentId { get; set; }

        public ICollection<DoctorSchedule>? DoctorSchedules { get; set; }
    }
}
