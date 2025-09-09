using AutoMapper;
using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Domain.Repositories.Vacancy;

namespace PlataformaEstagios.Application.UseCases.Vacancy.Update 
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

        public async Task<bool> UpdateVacancyAsync(Guid enterpriseId, Guid vacancyId, Communication.Requests.RequestUpdateVacancyJson request, CancellationToken ct)
        {
            var vacancy = await _readRepo.GetVacancyByIdForEnterpriseAsync(enterpriseId, vacancyId, ct);
            if (vacancy == null) return false;

            _mapper.Map(request, vacancy);
            vacancy.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateVacancyAsync(vacancy, ct);
            await _uow.Commit();
            return true;
        }
    }
}
