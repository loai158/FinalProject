using FinalProject.Core.Feature.Patient.Query.Response;
using MediatR;

namespace FinalProject.Core.Feature.Patient.Query.Models
{
    public class GetAllPatientQuery : IRequest<IEnumerable<GetAllPatientResponse>>
    {
        public string? Query { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
