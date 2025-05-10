using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using Microsoft.EntityFrameworkCore;
//[PrimaryKey(nameof(DoctorId), nameof(ApplicationUserId),nameof(AppointmentId))]
public class Cart
{
    public int Id { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }  
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; } 
    public TypePayment PaymentType { get; set; }
    public enum TypePayment
    {
        Online,
        Cash
    }

}



