using MediatR;

namespace FinalProject.Core.Feature.Patient.Command.Models
{
    public class DeletePatientCommand : IRequest<string>
    {
        public readonly int Id;

        public DeletePatientCommand(int id)
        {
            this.Id = id;
        }
    }
}
