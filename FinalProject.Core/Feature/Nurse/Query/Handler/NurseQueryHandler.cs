using FinalProject.Core.Feature.Nurse.Query.Models;
using FinalProject.Core.Feature.Nurse.Query.Response;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Nurse.Query.Handler
{
    public class NurseQueryHandler : IRequestHandler<GetNurseByIdQuery, NurseResponse>,
        IRequestHandler<GetAllNursesQuery, IEnumerable<GetAllNurseResponse>>
    {
        private readonly INurseServices _nurseServices;

        public NurseQueryHandler(INurseServices nurseServices)
        {
            this._nurseServices = nurseServices;
        }
        public async Task<NurseResponse> Handle(GetNurseByIdQuery request, CancellationToken cancellationToken)
        {
            var nurse = await _nurseServices.GetById(request.Id);
            var result = nurse.MapNurseResponse();
            return result;
        }

        public async Task<IEnumerable<GetAllNurseResponse>> Handle(GetAllNursesQuery request, CancellationToken cancellationToken)
        {
            var nurses = _nurseServices.GetAll();
            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var searchQuery = request.Query.ToLower();
                nurses = nurses
                    .Where(d => d.Name.ToLower().Contains(searchQuery) ||
                                d.Email.ToLower().Contains(searchQuery));
            }

            var totalCount = nurses.ToList().Count();
            var pagedNurses = nurses
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            var response = pagedNurses.MapNursesToGetAllNurseResponse();
            return response;
        }
    }
}
