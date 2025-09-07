using AutoMapper;
using PlataformaEstagios.Communication.Responses;
using PlataformaEstagios.Domain.Repositories.Enterprise;
using PlataformaEstagios.Domain.Repositories.Vacancy;

namespace PlataformaEstagios.Application.UseCases.Vacancy.GetVacancies
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
        public async Task<IReadOnlyList<Communication.Responses.ResponseVacancyListItem>> ExecuteAsync(Guid enterpriseId, CancellationToken ct)
        {
            var jobs = await _repo.GetActiveForEnterpriseAsync(enterpriseId, ct);
            return _mapper.Map<IReadOnlyList<Communication.Responses.ResponseVacancyListItem>>(jobs);
        }

        public async Task<IReadOnlyList<Communication.Responses.ResponseVacancyListItem>> ExecuteAsync(CancellationToken ct)
        {
            var jobs = await _repo.GetActiveForCandidateAsync(ct);

            var enterpriseIds = jobs.Select(j => j.EnterpriseIdentifier).Distinct().ToList();
            var enterprises = await _enterpriseRepo.GetNamesByIdsAsync(enterpriseIds, ct);
            // retorne algo como IEnumerable<(Guid Id, string Name)>

            var nameById = enterprises.ToDictionary(e => e.Id, e => e.Name);

            var mapped = _mapper.Map<List<ResponseVacancyListItem>>(jobs);
            foreach (var item in mapped)
                if (nameById.TryGetValue(item.EnterpriseIdentifier, out var name))
                    item.EnterpriseName = name;

            return mapped;
        }

        public async Task<Communication.Responses.ResponseGetVacancyJson> GetByIdAsync(Guid enterpriseId, Guid vacancyId, CancellationToken ct)
        {
            var job = await _repo.GetByIdForEnterpriseAsync(enterpriseId, vacancyId, ct);
            return _mapper.Map<Communication.Responses.ResponseGetVacancyJson>(job);
        }
    }
}
