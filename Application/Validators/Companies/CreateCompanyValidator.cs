using FluentValidation;
using Shared.Resources.Companies;

namespace Application.Validators.Companies
{
    public class CreateCompanyValidator : AbstractValidator<CompanyData>
    {
        public CreateCompanyValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name cannot be null or empty");
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Name length cannot be more than 50 character");
            RuleFor(x => x.CatchPhrase).MaximumLength(200).WithMessage("Catch Phrase length cannot be more than 200 character");
            RuleFor(x => x.BusinessSegment).MaximumLength(200).WithMessage("Business Segment length cannot be more than 200 character");

        }
    }
}
