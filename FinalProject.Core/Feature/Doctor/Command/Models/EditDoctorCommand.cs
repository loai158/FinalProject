using MediatR;

namespace FinalProject.Core.Feature.Doctor.Command.Models
{
    public class EditDoctorCommand : AddDoctorCommand, IRequest<int>
    {
        public EditDoctorCommand() { }
        public int Id { get; set; }
        public EditDoctorCommand(int id)
        {
            this.Id = id;
        }



    }
}
