using FinalProject.Data.Models.AppModels;

namespace FinalProject.Core.Feature.Doctor.Query.Response
{
    public class GetDoctorByIdResponse : GetAllDoctorsResponse
    {
        public List<Department> Departments { get; set; }
        public int DepartmentId { get; set; }
    }
}
