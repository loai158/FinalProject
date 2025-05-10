using FinalProject.Core.Feature.Doctor.Command.Models;
using FluentValidation;

namespace FinalProject.Core.Feature.Doctor.Command.Validations
{
    public class EditDoctorCommandValidator : AbstractValidator<EditDoctorCommand>
    {
        public EditDoctorCommandValidator()
        {


            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("الاسم مطلوب")
                .MaximumLength(100).WithMessage("الاسم لا يمكن أن يتجاوز 100 حرف");

            RuleFor(x => x.Details)
                .NotEmpty().WithMessage("التفاصيل مطلوبة")
                .MaximumLength(500).WithMessage("التفاصيل لا يمكن أن تتجاوز 500 حرف");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم الهاتف مطلوب")
                .Matches(@"^\+?\d{10,15}$").WithMessage("رقم الهاتف غير صالح");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("البريد الإلكتروني مطلوب")
                .EmailAddress().WithMessage("البريد الإلكتروني غير صالح");

            RuleFor(x => x.Gender)
                .NotNull().WithMessage("الجنس مطلوب");
            RuleFor(x => x.Image)
              .NotEmpty().WithMessage("الصوره مطلوب");
        }

    }
}
