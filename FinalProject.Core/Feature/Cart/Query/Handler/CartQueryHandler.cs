namespace FinalProject.Core.Feature.Cart.Query.Handler
{
    //public class CartQueryHandler : IRequestHandler<AddNewCartQuery, int>
    //{
    //    private readonly ICartServices _cartServices;
    //    private readonly IAppointmentServices _appointmentServices;

    //    public CartQueryHandler(ICartServices cartServices, IAppointmentServices appointmentServices)
    //    {
    //        this._cartServices = cartServices;
    //        this._appointmentServices = appointmentServices;
    //    }
    //    public Task<int> Handle(AddNewCartQuery request, CancellationToken cancellationToken)
    //    {
    //        // map first
    //        var cart = request.MapAddToAppointment();
    //        var appointment = request.MapAddToAppointment();
    //        // create the appoinment , Cart in DB
    //        var appointmentResult = _appointmentServices.Create(appointment);
    //        var cartResult = _cartServices.Create(cart);
    //        throw new NotImplementedException();
    //    }
    //}
}
