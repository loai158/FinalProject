namespace FinalProject.Core.CustomeServices.Cart
{

    public class CartServices : ICartServices { }
    //{
    //    private readonly IUnitOfWork _unitOfWork;

    //    public CartServices(IUnitOfWork unitOfWork)
    //    {
    //        _unitOfWork = unitOfWork;
    //    }

    //    public IEnumerable<GetAllResponce> GetAll()
    //    {
    //        var carts = _unitOfWork.CartRepository.Get(includes: [e => e.Doctor, e => e.Appointment]);
    //        return carts.Select(e => e.GetAll());
    //    }
    //    //public void AddCart(CartRequest cartRequest)
    //    //{
    //    //    var cart = cartRequest.CreateToCart();

    //    //    _unitOfWork.CartRepository.Create(cart);
    //    //    _unitOfWork.CartRepository.Commit();
    //    //}




}
