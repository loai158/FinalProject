using MediatR;

namespace FinalProject.Core.Feature.Cart.Query.Models
{
    public class AddNewCartQuery : IRequest<int>
    {

        //[Required(ErrorMessage = "ApplicationUserId is required.")]
        //public string userId { get; set; }

        //[Required(ErrorMessage = "DoctorId is required.")]
        //[Range(1, int.MaxValue, ErrorMessage = "DoctorId must be greater than 0.")]

        //public int DoctorId { get; set; }
        //[ForeignKey("DoctorId")]
        //public FinalProject.Data.Models.AppModels.Doctor Doctor { get; set; }

        //[Required(ErrorMessage = "DoctorSchedule is required.")]
        //public DoctorSchedule? doctorSchedule { get; set; }

        //[Required(ErrorMessage = "PaymentType is required.")]
        //[EnumDataType(typeof(TypePayment), ErrorMessage = "Invalid Payment Type.")]
        //public TypePayment PaymentType { get; set; }

        //[Required(ErrorMessage = "TypePrice is required.")]
        //[EnumDataType(typeof(Status), ErrorMessage = "Invalid Price Type.")]
        //public Status Status { get; set; }
    }
}
