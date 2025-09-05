using AutoMapper;
using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Domain.Repositories.Vacancy;

namespace PlataformaEstagios.Application.UseCases.Enterprise.UpdateVacancies
{
    public class UpdateVacancyUseCase : IUpdateVacancyUseCase
    {
        private readonly IVacancyWriteOnlyRepository _repo;
        private readonly IVacancyReadOnlyRepository _readRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public UpdateVacancyUseCase(IVacancyWriteOnlyRepository repo, IVacancyReadOnlyRepository readRepo, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _readRepo = readRepo;
            _mapper = mapper;
            _uow = unitOfWork;
        }

        public async Task<bool> ExecuteAsync(Guid enterpriseId, Guid vacancyId, Communication.Requests.RequestUpdateVacancyJson request, CancellationToken ct)
        {
            var vacancy = await _readRepo.GetByIdForEnterpriseAsync(enterpriseId, vacancyId, ct);
            if (vacancy == null) return false;

            _mapper.Map(request, vacancy);
            await _repo.UpdateAsync(vacancy, ct);
            await _uow.Commit();
            return true;
        }
    }
}
