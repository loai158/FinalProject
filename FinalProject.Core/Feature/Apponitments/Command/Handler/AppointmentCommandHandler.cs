using FinalProject.Core.Feature.Apponitments.Command.Models;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Apponitments.Command.Handler
{
    public class AppointmentCommandHandler : IRequestHandler<AddNewAppointmentCommand, int>,
        IRequestHandler<EditAppointmentCommand, bool>,
        IRequestHandler<DeleteAppointmentCommand, string>
    {
        private readonly IAppointmentServices _appointmentService;
        private readonly IDoctorServices doctorServices;

        public AppointmentCommandHandler(IAppointmentServices appointmentService, IDoctorServices doctorServices)
        {
            this._appointmentService = appointmentService;
            this.doctorServices = doctorServices;
        }
        public async Task<int> Handle(AddNewAppointmentCommand request, CancellationToken cancellationToken)
        {
            var doctor = await doctorServices.GetById(request.DoctorId);
            if (doctor == null)
                throw new Exception("Doctor not found.");

            var appointment = request.MapAddToAppointment(doctor);
            var result = await _appointmentService.Create(appointment);
            await doctorServices.update(request.SelectedScheduleId);
            return result;
        }

        public async Task<bool> Handle(EditAppointmentCommand request, CancellationToken cancellationToken)
        {
            //check if the Doctor  is exist first
            var appointment = await _appointmentService.GetById(request.Id);
            if (appointment == null)
            {
                return false;
            }
            //map
            var result = request.MapEditToAppointment();
            var final = _appointmentService.Edit(result);
            if (final == "success")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var nurse = await _appointmentService.GetById(request.Id);
            if (nurse == null)
                return "appointment not found";
            else
            {
                var result = _appointmentService.Delete(request.Id);

                if (await result.ConfigureAwait(false) == "faild")

                    return "faild";

                else
                    return "success";
            }
        }
    }
}
