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
                .NotEmpty().WithMessage("O nome de usuário é obrigatório.")
                .Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage("O nome de usuário não pode conter apenas espaços em branco.")
                .Length(3, 32).WithMessage("O nome de usuário deve ter entre 3 e 32 caracteres.")
                .Matches("^[a-zA-Z0-9._-]+$").WithMessage("O nome de usuário só pode conter letras, números, pontos (.), underlines (_) e hifens (-).");

            RuleFor(u => u.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O e-mail informado não é válido.")
                .MaximumLength(100).WithMessage("O e-mail deve ter no máximo 100 caracteres.");

            RuleFor(u => u.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(4).WithMessage("A senha deve ter pelo menos 4 caracteres.")
                .MaximumLength(200).WithMessage("A senha é muito longa.");

            RuleFor(u => u.UserType)
                .IsInEnum().WithMessage("O tipo de usuário informado é inválido.");

            // Caso o tipo seja Candidato
            When(u => u.UserType == UserType.Candidate, () =>
            {
                RuleFor(u => u.Candidate)
                    .NotNull().WithMessage("Os dados do candidato são obrigatórios para o tipo de usuário 'Candidato'.")
                    .SetValidator(new RequestCandidateValidator());
                RuleFor(u => u.Enterprise)
                    .Null().WithMessage("Os dados da empresa devem estar vazios para o tipo de usuário 'Candidato'.");
            });

            // Caso o tipo seja Empresa
            When(u => u.UserType == UserType.Enterprise, () =>
            {
                RuleFor(u => u.Enterprise)
                    .NotNull().WithMessage("Os dados da empresa são obrigatórios para o tipo de usuário 'Empresa'.")
                    .SetValidator(new RequestEnterpriseValidator());
                RuleFor(u => u.Candidate)
                    .Null().WithMessage("Os dados do candidato devem estar vazios para o tipo de usuário 'Empresa'.");
            });
        }
    }
}
