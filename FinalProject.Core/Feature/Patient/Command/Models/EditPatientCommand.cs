using FinalProject.Core.Feature.Patient.Query.Response;
using MediatR;

namespace FinalProject.Core.Feature.Patient.Command.Models
{
    public class EditPatientCommand : GetPatientByIdResponse, IRequest<bool>
    {
    }
}