using FinalProject.Core.Feature.Doctor.Query.Response;
using MediatR;

namespace FinalProject.Core.Feature.Doctor.Query.Models
{
    public class GetAllDoctorsQuery:IRequest<IEnumerable<GetAllDoctorsResponse>>
    {

    }
}
