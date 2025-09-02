using FluentValidation;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Domain.Enums;

namespace PlataformaEstagios.Application.UseCases.Vacancy.Create
{
    public class CreateVacancyValidator : AbstractValidator<RequestCreateVacancyJson>
    {
        public CreateVacancyValidator()
        {
            RuleFor(x => x.EnterpriseIdentifier).NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Título é obrigatório")
                .MaximumLength(120);

            RuleFor(x => x.Description)
                .MaximumLength(4000);

            // NOVOS
            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Localização é obrigatória")
                .MaximumLength(120);

            RuleFor(x => x.JobFunction)
                .IsInEnum().WithMessage("Função inválida");

            RuleFor(x => x.RequiredSkills)
                .NotEmpty().WithMessage("Informe ao menos uma habilidade necessária.");

            RuleForEach(x => x.RequiredSkills)
                .NotEmpty().MaximumLength(60);

            RuleFor(x => x.ExpiresAtUtc)
                .Must(expires => !expires.HasValue || expires.Value >= DateTime.UtcNow)
                .WithMessage("A data de expiração não pode ser no passado.");
        }
    }
}
