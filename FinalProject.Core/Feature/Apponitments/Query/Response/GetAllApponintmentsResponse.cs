namespace FinalProject.Core.Feature.Apponitments.Query.Response
{
    public class GetAllApponintmentsResponse
    {
        public List<GetAppointmentByIdResponse>? Appointments { get; set; } = new List<GetAppointmentByIdResponse>();
        public int TotalCount { get; set; }

    }
}
