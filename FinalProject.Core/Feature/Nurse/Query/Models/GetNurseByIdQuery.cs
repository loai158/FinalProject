using FinalProject.Core.Feature.Nurse.Query.Response;
using MediatR;

namespace FinalProject.Core.Feature.Nurse.Query.Models
{
    public class GetNurseByIdQuery:IRequest<NurseResponse>
    {
        public int Id { set; get;  }

       

    }
}
