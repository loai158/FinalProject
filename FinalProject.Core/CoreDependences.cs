using FinalProject.Core.Behavior;
using FinalProject.Core.Feature.Doctor.Command.Validators;
using FinalProject.Core.Feature.Patient.Command.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FinalProject.Core
{
    public static class CoreDependences
    {
        public static IServiceCollection AddCoreDependences(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<AddDoctorCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<AddNewPatientValidator>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });

            return services;
        }
    }
}
