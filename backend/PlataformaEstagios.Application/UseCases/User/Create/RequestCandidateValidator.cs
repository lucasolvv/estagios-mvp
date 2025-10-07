using FluentValidation;
using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagios.Application.UseCases.User.Create
{
    public class RequestCandidateValidator : AbstractValidator<RequestCandidateJson>
    {
        public RequestCandidateValidator()
        {
            RuleFor(c => c.FullName)
                .NotEmpty().WithMessage("Nome todo é obrigatório.")
                .MaximumLength(120);

            RuleFor(c => c.CourseName)
                .MaximumLength(120);

            RuleFor(c => c.BirthDate)
                .LessThan(DateTime.UtcNow.Date)
                .When(c => c.BirthDate.HasValue)
                .WithMessage("Data de nascimento precisa ser no passado.");

            // Address is optional; if present, validate it
            When(c => c.Address is not null, () =>
            {
                RuleFor(c => c.Address!).SetValidator(new RequestAddressValidator());
            });
        }
    }
}
