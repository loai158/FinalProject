

using FinalProject.Core.Feature.Nurse.Command.Models;
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
                DepartmentId = nurse.Department.Id,
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
        public static GetAllNurseResponse ToGetAllNurseResponse(this Nurse nurseResponse)
        {
            return new GetAllNurseResponse
            {
                Id = nurseResponse.Id,
                Name = nurseResponse.Name,
                Phone = nurseResponse.Phone,
                Image = nurseResponse.Image,
                Email = nurseResponse.Email,
                Department = nurseResponse.Department.Name,
            };
        }

        public static IEnumerable<GetAllNurseResponse> MapNursesToGetAllNurseResponse(this IEnumerable<Nurse> nurses)
        {
            return nurses.Select(nr => nr.ToGetAllNurseResponse());
        }
        public static Nurse MapAddToNurse(this AddNewNurseCommand nurseCommand)
        {
            return new Nurse
            {
                Name = nurseCommand.Name,
                Phone = nurseCommand.Phone,
                Image = nurseCommand.Image,
                Email = nurseCommand.Email,
                DepartmentId = nurseCommand.DepatrmentId
            };
        }
        public static Nurse MapEditToNurse(this EditNurseCommand nurseCommand)
        {
            return new Nurse
            {
                Name = nurseCommand.Name,
                Phone = nurseCommand.Phone,
                Image = nurseCommand.Image,
                Email = nurseCommand.Email,
                DepartmentId = nurseCommand.DepartmentId
            };
        }

    }
}
