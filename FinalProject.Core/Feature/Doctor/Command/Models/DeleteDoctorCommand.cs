using MediatR;

namespace FinalProject.Core.Feature.Doctor.Command.Models
{
    public class DeleteDoctorCommand : IRequest<string>
    {
        public readonly int Id;

        public DeleteDoctorCommand(int id)
        {
            this.Id = id;
        }
    }
}
