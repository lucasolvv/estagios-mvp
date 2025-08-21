using FluentValidation;
using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagios.Application.UseCases.User.Create
{
    public class RequestAddressValidator : AbstractValidator<RequestAddressJson>
    {
        public RequestAddressValidator()
        {
            RuleFor(a => a.Street)
                .MaximumLength(120);

            RuleFor(a => a.Complement)
                .MaximumLength(80);

            RuleFor(a => a.Neighborhood)
                .MaximumLength(80);

            RuleFor(a => a.City)
                .MaximumLength(80);

            RuleFor(a => a.UF)
                .Length(2).When(a => !string.IsNullOrWhiteSpace(a.UF))
                .WithMessage("UF must be 2 letters.");

            // Accepts "12345678" or "12345-678"
            RuleFor(a => a.Cep)
                .Matches(@"^\d{8}$|^\d{5}-\d{3}$")
                .When(a => !string.IsNullOrWhiteSpace(a.Cep))
                .WithMessage("CEP must be 8 digits or in the format 00000-000.");
        }
    }
}
