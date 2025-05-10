using FinalProject.Core.Feature.Nurse.Command.Models;
using FinalProject.Core.Mapping;
using FinalProject.Services.Abstracts;
using MediatR;

namespace FinalProject.Core.Feature.Nurse.Command.Handler
{
    public class NurseCommandHandler : IRequestHandler<AddNewNurseCommand, int>,
        IRequestHandler<EditNurseCommand, bool>,
        IRequestHandler<DeleteNurseCommand, string>
    {
        private readonly INurseServices _nurseServices;

        public NurseCommandHandler(INurseServices nurseServices)
        {
            this._nurseServices = nurseServices;
        }
        public async Task<int> Handle(AddNewNurseCommand request, CancellationToken cancellationToken)
        {

            var nurse = request.MapAddToNurse();
            var result = await _nurseServices.Create(nurse);
            return result;
        }

        public async Task<bool> Handle(EditNurseCommand request, CancellationToken cancellationToken)
        {
            //check if the Doctor  is exist first
            var doctor = await _nurseServices.GetById(request.Id);
            if (doctor == null)
            {
                return false;
            }
            //map
            var result = request.MapEditToNurse();
            var final = _nurseServices.Edit(result);
            if (final == "success")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> Handle(DeleteNurseCommand request, CancellationToken cancellationToken)
        {
            var nurse = await _nurseServices.GetById(request.Id);
            if (nurse.Image != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Nurses", nurse.Image);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }
            if (nurse == null)
                return "Nurse not found";
            else
            {
                var result = _nurseServices.Delete(request.Id);

                if (await result.ConfigureAwait(false) == "faild")

                    return "faild";

                else
                    return "success";
            }
        }
    }
}
