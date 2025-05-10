using FinalProject.Data.Models.AppModels;

namespace FinalProject.Core.Feature.Apponitments.Query.Response
{
    public class GetAllApponintmentsResponse
    {
        public int Id { get; set; }

        public decimal Price { get; set; }
        public DateOnly Date { get; set; }
        public int DoctorId { get; set; }
        public int DepartmentId { get; set; }
        public int PatientId { get; set; }
        public Status Status { get; set; }
        public ICollection<Perscribtion>? Perscribtions { get; set; }
    }
}
