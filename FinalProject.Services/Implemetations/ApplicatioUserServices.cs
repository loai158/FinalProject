using FinalProject.Data.Models.IdentityModels;
using FinalProject.Infrastructure.UnitOfWorks;
using FinalProject.Services.Abstracts;

namespace FinalProject.Services.Implemetations
{
    public class ApplicatioUserServices : IApplicatioUserServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicatioUserServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public List<string> GetEmails()
        {
            var response = _unitOfWork.Repositry<ApplicationUser>().Get().Select(x => x.Email);
            return response.ToList();
        }
    }
}
