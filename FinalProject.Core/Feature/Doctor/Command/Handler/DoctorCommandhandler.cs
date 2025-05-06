using FinalProject.Core.Feature.Doctor.Command.Models;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Doctor.Command.Handler
{
    public class DoctorCommandhandler : IRequestHandler<AddDoctorCommand, int>,
        IRequestHandler<EditDoctorCommand, int>

    {
        private readonly IDoctorServices _doctorServices;

        public DoctorCommandhandler(IDoctorServices doctorServices)
        {
            this._doctorServices = doctorServices;
        }
        public async Task<int> Handle(AddDoctorCommand request, CancellationToken cancellationToken)
        {
            if (await _doctorServices.IsDoctorNameExists(request.Name))
            {
                return -1;
            }
            //Map First

            var doctor = request.MapAddToDoctor();
            var result = await _doctorServices.Create(doctor);

            return result;

        }

        public async Task<int> Handle(EditDoctorCommand request, CancellationToken cancellationToken)
        {
            //check if the Doctor  is exist first
            var doctor = await _doctorServices.GetById(request.Id);
            if (doctor == null)
            {
                return -1;
            }
            //map
            var result = request.MapAddToDoctor();
            var final = _doctorServices.Edit(doctor);
            if (final == "success")
            {
                return result.Id;
            }
            else
            {
                return 0;
            }
        }
    }
}
