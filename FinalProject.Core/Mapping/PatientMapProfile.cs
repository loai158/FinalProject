using FinalProject.Core.Feature.Doctor.Query.Response;
using FinalProject.Core.Feature.Patient.Command.Models;
using FinalProject.Core.Feature.Patient.Query.Response;
using FinalProject.Data.Models.AppModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FinalProject.Core.Mapping
{
    public static class PatientMapProfile
    {
        public static Patient MapToPatient(this AddNewPatient patient)
        {
            return new Patient
            {
              Name = patient.Name,
                Address = patient.Address,
              DateOfBirth = patient.DateOfBirth,
             Email = patient.Email,
             Gender = patient.Gender,
                Image = patient.Image,
                Phone = patient.Phone,
                  PreviousMedicine = patient.PreviousMedicines?.Select(m => new PreviousMedicine
                  {
                      Name = m.Name,
                      Dose = m.Dose,
                      StartDate = m.StartDate,
                      EndDate = m.EndDate
                  }).ToList() ?? new List<PreviousMedicine>(),
                PreviousConditions = patient.PreviousConditions?.Select(c => new PreviousCondition
                {
                    Name = c.Name
                }).ToList() ?? new List<PreviousCondition>()
            };
        }
        public static GetAllPatientResponse MapPatientToResponse(this Patient patient)
        {
            return new GetAllPatientResponse
            {
                Name = patient.Name,
                Address = patient.Address,
                DateOfBirth = patient.DateOfBirth,
                Email = patient.Email,
                Gender = patient.Gender,
                Image = patient.Image,
                Phone = patient.Phone,
                PreviousMedicine = patient.PreviousMedicine,
                PreviousConditions = patient.PreviousConditions,

            };
        } 
        public static IEnumerable<GetAllPatientResponse> MapPatientsToResponse(this IEnumerable<Patient> patients)
        {
            return patients.Select(p => p.MapPatientToResponse());
        }
    }
}
