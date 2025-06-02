using FinalProject.Core.Feature.Doctor.Command.Models;
using FinalProject.Services.Abstracts;
using FluentValidation;

namespace FinalProject.Core.Feature.Doctor.Command.Validations
{
    public class AddDoctorCommandValidator : AbstractValidator<AddDoctorCommand>
    {
        public AddDoctorCommandValidator(IDoctorServices doctorServices)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("الاسم مطلوب.")
                .MaximumLength(100).WithMessage("الاسم يجب ألا يزيد عن 100 حرف.");


            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("البريد الإلكتروني مطلوب.")
                .EmailAddress().WithMessage("صيغة البريد الإلكتروني غير صحيحة.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم الهاتف مطلوب.")
                .Matches(@"^01[0-2,5]{1}[0-9]{8}$").WithMessage("رقم الهاتف غير صحيح.");


            RuleFor(x => x.Details)
                .MaximumLength(500).WithMessage("التفاصيل يجب ألا تزيد عن 500 حرف.");

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage("الصورة مطلوبة.");
            RuleFor(x => x.DepatrmentId)
    .GreaterThan(0).WithMessage("القسم مطلوب.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("الجنس غير صحيح.");
        }
    }
}
