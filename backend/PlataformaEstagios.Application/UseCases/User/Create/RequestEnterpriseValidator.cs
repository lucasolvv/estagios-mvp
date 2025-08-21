using FluentValidation;
using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagios.Application.UseCases.User.Create
{
    public class RequestEnterpriseValidator : AbstractValidator<RequestEnterpriseJson>
    {
        public RequestEnterpriseValidator()
        {
            RuleFor(e => e.TradeName)
                .NotEmpty().WithMessage("TradeName is required.")
                .MaximumLength(150);

            // Accepts 14 digits with or without punctuation (00.000.000/0000-00 or 00000000000000)
            RuleFor(e => e.Cnpj)
                .NotEmpty().WithMessage("CNPJ is required.")
                .Must(IsCnpjDigits14).WithMessage("CNPJ must contain 14 digits (punctuation allowed).");

            RuleFor(e => e.ActivityArea)
                .MaximumLength(120);

            When(e => e.Address is not null, () =>
            {
                RuleFor(e => e.Address!).SetValidator(new RequestAddressValidator());
            });
        }

        private static bool IsCnpjDigits14(string? cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return false;
            var digits = new string(cnpj.Where(char.IsDigit).ToArray());
            return digits.Length == 14;
        }
    }
}
