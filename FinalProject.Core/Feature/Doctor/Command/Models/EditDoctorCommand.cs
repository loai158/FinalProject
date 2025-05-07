using FinalProject.Core.Feature.Doctor.Query.Response;
using MediatR;

namespace FinalProject.Core.Feature.Doctor.Command.Models
{
    public class EditDoctorCommand : GetDoctorByIdResponse, IRequest<bool>
    {

    }
}
