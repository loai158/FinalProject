using FinalProject.Core.Feature.Patient.Command.Models;
using FinalProject.Core.Feature.Patient.Query.Response;
using FinalProject.Data.Models.AppModels;

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
        public static IEnumerable<GetAllPatientResponse> MapPatientsToResponse(this IEnumerable<Patient> patients)
        {
            return patients.Select(p => p.MapPatientToResponse());
        }
        public static GetAllPatientResponse MapPatientToResponse(this Patient patient)
        {
            return new GetAllPatientResponse
            {
                Id = patient.Id,
                Name = patient.Name,
                Address = patient.Address,
                DateOfBirth = patient.DateOfBirth,
                Email = patient.Email,
                Gender = patient.Gender,
                Image = patient.Image,
                Phone = patient.Phone,
                Appointments = patient.Appointments,
                PreviousMedicine = patient.PreviousMedicine?.Select(m => new PreviousMedicine
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

        public static GetPatientByIdResponse PatientToResponse(this Patient patient)
        {
            return new GetPatientByIdResponse
            {
                Id = patient.Id,
                Name = patient.Name,
                Address = patient.Address,
                DateOfBirth = patient.DateOfBirth,
                Email = patient.Email,
                Gender = patient.Gender,
                Image = patient.Image,
                Phone = patient.Phone,
                PreviousMedicine = patient.PreviousMedicine?.Select(m => new PreviousMedicine
                {
                    Name = m.Name,
                    Dose = m.Dose,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate
                }).ToList() ?? new List<PreviousMedicine>(),
                PreviousConditions = patient.PreviousConditions?.Select(c => new PreviousCondition
                {
                    Name = c.Name
                }).ToList() ?? new List<PreviousCondition>(),
                Appointments = patient.Appointments?.Select(c => new Appointment
                {
                    Date = c.Date,
                    Status = c.Status,
                }).ToList() ?? new List<Appointment>(),
            };

        }
        public static Patient MapEditToPatient(this EditPatientCommand patient)
        {
            return new Patient
            {
                Id = patient.Id,
                Name = patient.Name,
                Address = patient.Address,
                DateOfBirth = patient.DateOfBirth,
                Email = patient.Email,
                Gender = patient.Gender,
                Image = patient.Image,
                Phone = patient.Phone,
                PreviousMedicine = patient.PreviousMedicine?.Select(m => new PreviousMedicine
                {
                    Name = m.Name,
                    Dose = m.Dose,
                    StartDate = m.StartDate,
                    EndDate = (DateTime)m.EndDate
                }).ToList() ?? new List<PreviousMedicine>(),
                PreviousConditions = patient.PreviousConditions?.Select(c => new PreviousCondition
                {
                    Name = c.Name
                }).ToList() ?? new List<PreviousCondition>()
            };

        }
    }
}
