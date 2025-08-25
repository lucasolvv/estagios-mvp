using FluentValidation;
using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagios.Application.UseCases.Auth.Login
{
    public class LoginRequestValidator : AbstractValidator<RequestLoginJson>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.EmailOrNickname)
                    .NotEmpty().WithMessage("Email or Nickname is required.")
                    .MaximumLength(100);

            RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MaximumLength(200);
        }
    }
}
