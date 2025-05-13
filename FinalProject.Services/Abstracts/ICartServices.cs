using FinalProject.Data.Models.PaymentModels;

namespace FinalProject.Services.Abstracts
{
    public interface ICartServices
    {
        public Task<int> Create(Cart cart);
        //    IEnumerable<GetAllResponce> GetAll();
    }
}
