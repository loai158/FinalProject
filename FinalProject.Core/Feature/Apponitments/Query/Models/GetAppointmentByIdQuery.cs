using FinalProject.Core.Feature.Apponitments.Query.Response;
using MediatR;

namespace FinalProject.Core.Feature.Apponitments.Query.Models
{
    public class GetAppointmentByIdQuery : IRequest<GetAppointmentByIdResponse>
    {
        public int Id { get; set; }
        public GetAppointmentByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}
