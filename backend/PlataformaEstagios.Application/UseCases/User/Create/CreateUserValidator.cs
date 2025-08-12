using FluentValidation;
using PlataformaEstagios.Communication.Requests;
namespace PlataformaEstagios.Application.UseCases.User.Create
{
    public class CreateUserValidator : AbstractValidator<ResquestCreateUserJson>
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
                .MinimumLength(32).WithMessage("Password looks invalid (too short).")
                .MaximumLength(200).WithMessage("Password looks invalid (too long).");

            //RuleFor(u => u.UserType)
            //    .IsInEnum().WithMessage("UserType is invalid.");
        }
    }
}
