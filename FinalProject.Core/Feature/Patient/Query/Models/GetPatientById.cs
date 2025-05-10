using FinalProject.Core.Feature.Patient.Query.Response;
using MediatR;

namespace FinalProject.Core.Feature.Patient.Query.Models
{
    public class GetPatientById : IRequest<GetPatientByIdResponse>
    {
        public int Id { get; set; }
        public GetPatientById(int id)
        {
            this.Id = id;
        }
    }
}
