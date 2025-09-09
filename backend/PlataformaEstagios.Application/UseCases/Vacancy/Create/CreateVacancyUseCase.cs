using System.Linq;
using AutoMapper;
using FluentValidation;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Communication.Responses;
using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Domain.Repositories.Enterprise;
using PlataformaEstagios.Domain.Repositories.Vacancy;
using PlataformaEstagios.Exceptions.ExceptionBase;

namespace PlataformaEstagios.Application.UseCases.Vacancy.Create
{
    public class CreateVacancyUseCase : ICreateVacancyUseCase
    {
        private readonly IValidator<RequestCreateVacancyJson> _validator;
        private readonly IEnterpriseReadOnlyRepository _enterpriseRepo;
        private readonly IVacancyWriteOnlyRepository _vacancyRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateVacancyUseCase(
            IValidator<RequestCreateVacancyJson> validator,
            IEnterpriseReadOnlyRepository enterpriseRepo,
            IVacancyWriteOnlyRepository vacancyRepo,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _validator = validator;
            _enterpriseRepo = enterpriseRepo;
            _vacancyRepo = vacancyRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResponseCreateVacancyJson> ExecuteAsync(RequestCreateVacancyJson request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            if (!await _enterpriseRepo.ExistsAsync(request.EnterpriseIdentifier, ct))
                throw new ErrorOnValidationException(new List<string> { "Empresa inválida ou inexistente." });

            var entity = _mapper.Map<Domain.Entities.Vacancy>(request);

            entity.VacancyIdentifier = Guid.NewGuid();
            entity.UpdatedAt = DateTime.UtcNow;

            // Publicação e Skills CSV
            entity.PublishedAtUtc = DateTime.UtcNow;
            entity.RequiredSkillsCsv = string.Join(", ",
                (request.RequiredSkills ?? new())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim()));

            await _vacancyRepo.AddVacancyAsync(entity, ct);
            await _uow.Commit();

            return _mapper.Map<ResponseCreateVacancyJson>(entity);
        }
    }
}
