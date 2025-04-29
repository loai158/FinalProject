using FinalProject.Core.Feature.Nurse.Query.Models;
using FinalProject.Core.Feature.Nurse.Query.Response;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Nurse.Query.Handler
{
    public class NurseQueryHandler : IRequestHandler<GetNurseByIdQuery, NurseResponse>
    {
        private readonly INurseServices _nurseServices;

        public NurseQueryHandler(INurseServices nurseServices )
        {
            this._nurseServices = nurseServices;
        }
        public async Task<NurseResponse> Handle(GetNurseByIdQuery request, CancellationToken cancellationToken)
        {
             var nurse = await _nurseServices.GetById( request.Id );
            var result = nurse.MapNurseResponse();
            return result;
        }
    }
}
