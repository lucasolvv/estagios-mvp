using FluentValidation;
using PlataformaEstagios.Communication.Requests;

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
        }
    }
}
