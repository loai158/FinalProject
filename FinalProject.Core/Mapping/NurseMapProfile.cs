

using FinalProject.Core.Feature.Nurse.Query.Response;
using FinalProject.Data.Models.AppModels;

namespace FinalProject.Core.Mapping
{
    public static class NurseMapProfile
    {

        public static NurseResponse MapNurseResponse(this Nurse nurse)
        {
            return new NurseResponse
            {
                Id = nurse.Id,
                Name = nurse.Name,
                Department = nurse.Department.Name,
                Email = nurse.Email,
                Phone = nurse.Phone,
                Image = nurse.Image,
                MedicalRecords = nurse.MedicalRecords,
            };

        }
        public static IEnumerable<NurseResponse> MapNursesResponseDTOs(this IEnumerable<Nurse> nurses)
        {
            return nurses.Select(c => c.MapNurseResponse());
        }
    } 
}
