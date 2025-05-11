using MediatR;

namespace FinalProject.Core.Feature.Apponitments.Command.Models
{
    public class DeleteAppointmentCommand : IRequest<string>
    {
        public readonly int Id;

        public DeleteAppointmentCommand(int id)
        {
            this.Id = id;
        }
    }

}
