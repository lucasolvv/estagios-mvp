using FluentValidation;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Domain.Enums;

namespace PlataformaEstagios.Application.UseCases.User.Create
{
    public class CreateUserValidator : AbstractValidator<RequestCreateUserJson>
    {
        public CreateUserValidator()
        {
            RuleFor(u => u.Nickname)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("NickName is Required")
                .Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage("Nickname cannot be whitespace.")
                .Length(3, 32).WithMessage("Nickname must be between 3 and 32 characters.")
                .Matches("^[a-zA-Z0-9._-]+$").WithMessage("Nickname can contain only letters, numbers, '.', '_' and '-'.");

            RuleFor(u => u.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is invalid.")
                .MaximumLength(100).WithMessage("Email must be at most 100 characters.");

            RuleFor(u => u.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(4).WithMessage("Password looks invalid (too short). Minimal 4 chars")
                .MaximumLength(200).WithMessage("Password looks invalid (too long).");

            RuleFor(u => u.UserType)
                .IsInEnum().WithMessage("UserType is invalid.");

            // Conditional: Candidate
            When(u => u.UserType == UserType.Candidate, () =>
            {
                RuleFor(u => u.Candidate)
                    .NotNull().WithMessage("Candidate payload is required for UserType.Candidate.")
                    .SetValidator(new RequestCandidateValidator());
                RuleFor(u => u.Enterprise)
                    .Null().WithMessage("Enterprise payload must be null for UserType.Candidate.");
            });

            // Conditional: Enterprise
            When(u => u.UserType == UserType.Enterprise, () =>
            {
                RuleFor(u => u.Enterprise)
                    .NotNull().WithMessage("Enterprise payload is required for UserType.Enterprise.")
                    .SetValidator(new RequestEnterpriseValidator());
                RuleFor(u => u.Candidate)
                    .Null().WithMessage("Candidate payload must be null for UserType.Enterprise.");
            });
        }
    }
}
