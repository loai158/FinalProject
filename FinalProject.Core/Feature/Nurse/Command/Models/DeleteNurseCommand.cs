using MediatR;

namespace FinalProject.Core.Feature.Nurse.Command.Models
{
    public class DeleteNurseCommand : IRequest<string>
    {
        public readonly int Id;

        public DeleteNurseCommand(int id)
        {
            this.Id = id;
        }
    }
}
