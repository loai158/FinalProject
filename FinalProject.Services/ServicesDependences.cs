using FinalProject.Infrastructure.IRepositry;
using FinalProject.Infrastructure.Repositry;
using FinalProject.Services.Abstracts;
using FinalProject.Services.Implemetations;
using Microsoft.Extensions.DependencyInjection;

namespace FinalProject.Services
{
    public static class ServicesDependences
    {
        public static IServiceCollection AddServicesDependences(this IServiceCollection services)
        {
            services.AddScoped<IDoctorServices, DoctorServices>();
            services.AddScoped<INurseServices, NurseServices>();
            services.AddScoped<IPatientServices, PatientServices>();
            services.AddScoped<IDepartmentServices, DepartmentServices>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IAppointmentServices, AppointmentServices>();
            services.AddScoped<IApplicatioUserServices, ApplicatioUserServices>();
            services.AddScoped<IPreviousConditionServices, PreviousConditionServices>();
            services.AddScoped<IPreviousMedicineServices, PreviousMedicineServices>();
            services.AddScoped<IPrescriptionService, PrescriptionService>();
            services.AddScoped<IMedicineServices, MedicineServices>();
            services.AddScoped<IPerscribtionMedicineService, PerscribtionMedicineService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IScheduleServices, ScheduleServices>();


            return services;
        }
    }
}
