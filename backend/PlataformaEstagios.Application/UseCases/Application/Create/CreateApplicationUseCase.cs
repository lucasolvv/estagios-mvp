using PlataformaEstagios.Domain.Repositories.Application;
using PlataformaEstagios.Communication.Requests;
using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Domain.Enums;
using AutoMapper;
using PlataformaEstagios.Domain.Repositories.Vacancy;

namespace PlataformaEstagios.Application.UseCases.Application.Create
{
    public sealed class CreateApplicationUseCase : ICreateApplicationUseCase
    {
        private readonly IApplicationWriteOnlyRepository _appWrite;
        private readonly IApplicationReadOnlyRepository _appRead;
        private readonly IVacancyReadOnlyRepository _vacRead;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateApplicationUseCase(
            IApplicationWriteOnlyRepository appWrite,
            IUnitOfWork uow,
            IMapper mapper,
            IVacancyReadOnlyRepository vacRead,
            IApplicationReadOnlyRepository appRead
            )
        {
            _appWrite = appWrite;
            _uow = uow;
            _mapper = mapper;
            _vacRead = vacRead;
            _appRead = appRead;
        }
        public async Task CreateNewCandidateApplication(Guid vacancyId, Guid candidateId, CancellationToken ct)
        {
            // 1) Validar inputs (Guid.Empty etc) – também faça com FluentValidation na request
            if (vacancyId == Guid.Empty || candidateId == Guid.Empty)
                throw new ArgumentException("IDs inválidos");

            // 2) Verificar se a vaga existe e está ativa (e não expirada, se aplicável)
            var vacancy = await _vacRead.GetVacancyByIdForCandidateAsync(vacancyId, ct);
            if (vacancy is null || !vacancy.IsActive /* || está expirada */)
                throw new InvalidOperationException("Vaga indisponível.");

            // 3) Impedir duplicidade (um candidato por vaga)
            var applicationAlreadyExists = await _appRead.ExistsAsync(vacancyId, candidateId);
            if (applicationAlreadyExists) throw new InvalidOperationException("Candidatura já existe para esta vaga.");

            // 4) Criar entidade
            var entity = new Domain.Entities.Application
            {
                ApplicationIdentifier = Guid.NewGuid(),
                VacancyId = vacancyId,
                CandidateIdentifier = candidateId,
                ApplicationDate = DateTime.UtcNow,
                Status = ApplicationStatus.Pending
            };

            await _appWrite.AddAsync(entity, ct);
            await _uow.Commit();
        }
    }
}
