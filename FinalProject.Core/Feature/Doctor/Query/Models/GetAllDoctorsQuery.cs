using FinalProject.Core.Feature.Doctor.Query.Response;
using MediatR;

namespace FinalProject.Core.Feature.Doctor.Query.Models
{
    public class GetAllDoctorsQuery : IRequest<IEnumerable<GetAllDoctorsResponse>>
    {
        public string? Query { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
