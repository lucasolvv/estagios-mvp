using FluentValidation;
using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagios.Application.UseCases.User.Create
{
    public class RequestCandidateValidator : AbstractValidator<RequestCandidateJson>
    {
        public RequestCandidateValidator()
        {
            RuleFor(c => c.FullName)
                .NotEmpty().WithMessage("FullName is required.")
                .MaximumLength(120);

            RuleFor(c => c.CourseName)
                .MaximumLength(120);

            RuleFor(c => c.BirthDate)
                .LessThan(DateTime.UtcNow.Date)
                .When(c => c.BirthDate.HasValue)
                .WithMessage("BirthDate must be in the past.");

            // Address is optional; if present, validate it
            When(c => c.Address is not null, () =>
            {
                RuleFor(c => c.Address!).SetValidator(new RequestAddressValidator());
            });
        }
    }
}
