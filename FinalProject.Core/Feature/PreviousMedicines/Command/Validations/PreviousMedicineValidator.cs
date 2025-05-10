using FinalProject.Data.Models.AppModels;
using FluentValidation;

namespace FinalProject.Core.Feature.PreviousMedicines.Command.Validations
{
    public class PreviousMedicineValidator : AbstractValidator<PreviousMedicine>
    {
        public PreviousMedicineValidator()
        {
            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("تاريخ البداية يجب أن يكون قبل تاريخ النهاية");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("تاريخ النهاية مطلوب");
        }
    }
}
