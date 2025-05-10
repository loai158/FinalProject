using FinalProject.Core.Feature.Doctor.Command.Models;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Doctor.Command.Handler
{
    public class DoctorCommandhandler : IRequestHandler<AddDoctorCommand, int>,
        IRequestHandler<EditDoctorCommand, bool>,
        IRequestHandler<DeleteDoctorCommand, string>

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

        public async Task<bool> Handle(EditDoctorCommand request, CancellationToken cancellationToken)
        {
            //check if the Doctor  is exist first
            var doctor = await _doctorServices.GetById(request.Id);
            if (doctor == null)
            {
                return false;
            }
            //map
            var result = request.MapEditToDoctor();
            var final = _doctorServices.Edit(result);
            if (final == "success")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _doctorServices.GetById(request.Id);
            if (doctor.Image != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Doctors", doctor.Image);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }
            if (doctor == null)
                return "doctor not found";
            else
            {
                var result = _doctorServices.Delete(request.Id);

                if (await result.ConfigureAwait(false) == "faild")

                    return "faild";

                else
                    return "success";
            }

        }
    }
}
