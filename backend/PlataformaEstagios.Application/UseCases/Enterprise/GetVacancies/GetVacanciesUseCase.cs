using AutoMapper;
using PlataformaEstagios.Domain.Repositories.Vacancy;

namespace PlataformaEstagios.Application.UseCases.Enterprise.GetVacancies
{
    public class GetVacanciesUseCase : IGetVacanciesUseCase
    {
        private readonly IVacancyReadOnlyRepository _repo;
        private readonly IMapper _mapper;
        public GetVacanciesUseCase(IVacancyReadOnlyRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<Communication.Responses.ResponseVacancyListItem>> ExecuteAsync(Guid enterpriseId, CancellationToken ct)
        {
            var jobs = await _repo.GetActiveForEnterpriseAsync(enterpriseId, ct);
            return _mapper.Map<IReadOnlyList<Communication.Responses.ResponseVacancyListItem>>(jobs);
        }

        public async Task<Communication.Responses.ResponseGetVacancyJson> GetByIdAsync(Guid enterpriseId, Guid vacancyId, CancellationToken ct)
        {
            var job = await _repo.GetByIdForEnterpriseAsync(enterpriseId, vacancyId, ct);
            return _mapper.Map<Communication.Responses.ResponseGetVacancyJson>(job);
        }
    }
}
