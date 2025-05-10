using FinalProject.Core.Feature.Patient.Command.Models;
using FluentValidation;

namespace FinalProject.Core.Feature.Patient.Command.Validations
{
    public class EditPatientCommandValidator : AbstractValidator<EditPatientCommand>
    {
        public EditPatientCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Gender).NotEmpty();
            RuleFor(x => x.DateOfBirth).NotEmpty();
            RuleForEach(x => x.PreviousConditions)
                  .ChildRules(condition =>
                  {
                      condition.RuleFor(c => c.Name)
                          .IsInEnum()
                          .WithMessage("يرجى اختيار حالة طبية صالحة (مثل السكري أو ارتفاع ضغط الدم)");
                  })
                  .When(x => x.PreviousConditions != null && x.PreviousConditions.Any());
            RuleForEach(x => x.PreviousMedicine).ChildRules(medicine =>
            {
                medicine.RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
                medicine.RuleFor(m => m.StartDate).NotEmpty();
                medicine.RuleFor(m => m.EndDate).GreaterThanOrEqualTo(m => m.StartDate);

            });
        }

    }
}
