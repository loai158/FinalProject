using FinalProject.Core.Feature.Apponitments.Query.Response;
using MediatR;

namespace FinalProject.Core.Feature.Apponitments.Query.Models
{
    public class GetAllApponintmentsByDoctorIdQuery : IRequest<GetAllApponintmentsResponse>
    {
        public int doctorId { get; set; }
        public string? Query { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
