using AutoMapper;
using PlataformaEstagios.Domain.Repositories.Application;
using PlataformaEstagios.Communication.Responses;
using PlataformaEstagios.Domain.Repositories.Enterprise;

namespace PlataformaEstagios.Application.UseCases.Application.Get
{
    public class GetApplicationUseCase : IGetApplicationUseCase
    {
        private readonly IApplicationReadOnlyRepository _appReadRepo;
        private readonly IEnterpriseReadOnlyRepository _enterpriseRepo;
        private readonly IMapper _mapper;

        public GetApplicationUseCase(IApplicationReadOnlyRepository appReadRepo, IMapper mapper, IEnterpriseReadOnlyRepository enterpriseReadRepo)
        {
            _appReadRepo = appReadRepo;
            _mapper = mapper;
            _enterpriseRepo = enterpriseReadRepo;
        }

        public async Task<IReadOnlyList<ResponseGetApplicationJson>> GetRecentApplicationsByCandidateIdAsync(Guid candidateId)
        {
            var applications = await _appReadRepo.GetRecentApplicationsByCandidateIdAsync(candidateId);
            if (applications == null || applications.Count == 0)
                return Array.Empty<ResponseGetApplicationJson>();
            var mappedApplications = _mapper.Map<IReadOnlyList<ResponseGetApplicationJson>>(applications);

            var idx = mappedApplications.ToDictionary(x => x.ApplicationIdentifier);

            foreach (var app in applications)
            {
                if (app.Vacancy != null && idx.TryGetValue(app.ApplicationIdentifier, out var dto))
                {
                    dto.TituloVaga = app.Vacancy.Title!;
                    dto.NomeEmpresa = _enterpriseRepo.GetEnterpriseNameByIdAsync(app.Vacancy.EnterpriseIdentifier).Result;
                }
            }


            return mappedApplications;
        }
    }
}
