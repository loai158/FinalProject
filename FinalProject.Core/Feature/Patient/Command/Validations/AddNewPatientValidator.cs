using FinalProject.Core.Feature.Patient.Command.Models;
using FinalProject.Core.Feature.PreviousMedicines.Command.Validations;
using FluentValidation;


namespace FinalProject.Core.Feature.Patient.Command.Validations
{
    public class AddNewPatientValidator : AbstractValidator<AddNewPatient>
    {
        public AddNewPatientValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم المريض مطلوب.")
                .MaximumLength(100).WithMessage("اسم المريض لا يجب أن يتجاوز 100 حرف.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم الهاتف مطلوب.");


            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("البريد الإلكتروني مطلوب.")
                .EmailAddress().WithMessage("صيغة البريد الإلكتروني غير صحيحة.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("العنوان مطلوب.")
                .MaximumLength(200).WithMessage("العنوان طويل جدًا.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("الجنس غير صحيح.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Today).WithMessage("تاريخ الميلاد يجب أن يكون في الماضي.");

            RuleForEach(x => x.PreviousMedicines).SetValidator(new PreviousMedicineValidator());


        }
    }

}

