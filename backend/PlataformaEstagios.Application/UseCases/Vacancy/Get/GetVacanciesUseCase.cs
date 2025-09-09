using AutoMapper;
using PlataformaEstagios.Communication.Responses;
using PlataformaEstagios.Domain.Repositories.Enterprise;
using PlataformaEstagios.Domain.Repositories.Vacancy;

namespace PlataformaEstagios.Application.UseCases.Vacancy.Get
{
    public class GetVacanciesUseCase : IGetVacanciesUseCase
    {
        private readonly IVacancyReadOnlyRepository _repo;
        private readonly IEnterpriseReadOnlyRepository _enterpriseRepo;
        private readonly IMapper _mapper;
        public GetVacanciesUseCase(IVacancyReadOnlyRepository repo, IEnterpriseReadOnlyRepository enterpriseReadOnlyRepository, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _enterpriseRepo = enterpriseReadOnlyRepository;
        }
        public async Task<IReadOnlyList<ResponseVacancyListItem>> GetAllActiveVacanciesForEnterpriseAsync(Guid enterpriseId, CancellationToken ct)
        {
            var jobs = await _repo.GetActiveVacanciesForEnterpriseAsync(enterpriseId, ct);
            return _mapper.Map<IReadOnlyList<ResponseVacancyListItem>>(jobs);
        }

        public async Task<IReadOnlyList<ResponseVacancyListItem>> GetAllActiveVacanciesForCandidateAsync(CancellationToken ct)
        {
            var jobs = await _repo.GetActiveVacanciesForCandidateAsync(ct);

            var mappedJobs = _mapper.Map<List<ResponseVacancyListItem>>(jobs);

            // índice por Id para achar em O(1) em vez de varrer toda a lista
            var idx = mappedJobs.ToDictionary(x => x.VacancyIdentifier);

            foreach (var job in jobs)
            {
                var enterprise = await _enterpriseRepo.GetEnterpriseNameByIdAsync(job.EnterpriseIdentifier /*, ct?*/);
                if (enterprise != null && idx.TryGetValue(job.VacancyIdentifier, out var dto))
                    dto.EnterpriseName = enterprise;
            }

            return mappedJobs;
        }

        public async Task<ResponseGetVacancyJson> GetVacancyByIdForEnterpriseAsync(Guid enterpriseId, Guid vacancyId, CancellationToken ct)
        {
            var job = await _repo.GetVacancyByIdForEnterpriseAsync(enterpriseId, vacancyId, ct);
            return _mapper.Map<ResponseGetVacancyJson>(job);
        }

        public async Task<ResponseGetVacancyToApplicationJson> GetVacancyByIdForCandidateAsync(Guid vacancyId, CancellationToken ct)
        {
            var job = await _repo.GetVacancyByIdForCandidateAsync(vacancyId, ct);
            var mappedJob = _mapper.Map<ResponseGetVacancyToApplicationJson>(job);

            if (job != null)
            {
                var enterprise = await _enterpriseRepo.GetEnterpriseNameByIdAsync(job.EnterpriseIdentifier);
                mappedJob.EnterpriseName = enterprise;
            }

            return mappedJob;
        }
    }
}
