using FinalProject.Data.Models.AppModels;

namespace FinalProject.Core.Feature.Doctor.Query.Response
{
    public class GetAllDoctorsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Phone { get; set; }
        public string Department{ get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public ICollection<DoctorSchedule>? DoctorSchedules { get; set; }


    }
}
