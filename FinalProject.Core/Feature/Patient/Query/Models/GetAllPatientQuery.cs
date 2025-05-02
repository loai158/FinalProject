using FinalProject.Core.Feature.Patient.Query.Response;
using MediatR;

namespace FinalProject.Core.Feature.Patient.Query.Models
{
    public class GetAllPatientQuery:IRequest<IEnumerable<GetAllPatientResponse>>
    {

    }
}
