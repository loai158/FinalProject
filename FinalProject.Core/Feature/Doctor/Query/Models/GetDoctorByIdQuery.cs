using FinalProject.Core.Feature.Doctor.Query.Response;
using MediatR;

namespace FinalProject.Core.Feature.Doctor.Query.Models
{
    public class GetDoctorByIdQuery : IRequest<GetDoctorByIdResponse>
    {
        public int Id { get; set; }
        public GetDoctorByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}
