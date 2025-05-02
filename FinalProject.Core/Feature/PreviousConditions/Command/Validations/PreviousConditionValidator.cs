using FinalProject.Data.Models.AppModels;
using FluentValidation;

namespace FinalProject.Core.Feature.PreviousConditions.Command.Validations
{
    public class PreviousConditionValidator : AbstractValidator<PreviousCondition>
    {
        public PreviousConditionValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("اسم الحالة المرضية مطلوب.");
        }
    }
}