using FinalProject.Core.Feature.Nurse.Query.Response;
using MediatR;

namespace FinalProject.Core.Feature.Nurse.Query.Models
{
    public class GetAllNursesQuery : IRequest<IEnumerable<GetAllNurseResponse>>
    {
        public string? Query { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
