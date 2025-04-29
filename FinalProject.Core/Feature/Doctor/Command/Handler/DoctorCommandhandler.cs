using FinalProject.Core.Feature.Doctor.Command.Models;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Feature.Doctor.Command.Handler
{
    public class DoctorCommandhandler : IRequestHandler<AddDoctorCommand, int>
    {
        private readonly IDoctorServices _doctorServices;

        public DoctorCommandhandler( IDoctorServices doctorServices  )
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
            var result =await _doctorServices.Create(doctor);
            
                return result;
            
        }
    }
}
