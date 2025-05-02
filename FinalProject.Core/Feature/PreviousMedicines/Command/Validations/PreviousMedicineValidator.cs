using FluentValidation;
using FinalProject.Data.Models.AppModels;
using FinalProject.Core.Feature.PreviousMedicines.Command.Models;

namespace FinalProject.Core.Feature.PreviousMedicines.Command.Validations
{
    public class PreviousMedicineValidator : AbstractValidator<PreviousMedicine>
    {
        public PreviousMedicineValidator()
        {
            RuleFor(x => x.Name)
                   .NotEmpty().WithMessage("اسم الدواء مطلوب");

            RuleFor(x => x.Dose)
                .NotEmpty().WithMessage("الجرعة مطلوبة");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("تاريخ البداية مطلوب")
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("تاريخ البداية يجب أن يكون قبل تاريخ النهاية");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("تاريخ النهاية مطلوب");
        }
    }
}
