using AutoMapper;
using PlataformaEstagios.Communication.Responses;
using PlataformaEstagios.Domain.Repositories.Candidate;

namespace PlataformaEstagios.Application.UseCases.Candidate.Get
{
    public class GetCandidateUseCase : IGetCandidateUseCase
    {
        private readonly IMapper _mapper;
        private readonly ICandidateReadOnlyRepository _repo;

        public GetCandidateUseCase(IMapper mapper, ICandidateReadOnlyRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ResponseGetCandidateProfileJson> GetCandidateByIdAsync(Guid candidateId)
        {
            if (candidateId == Guid.Empty) throw new ArgumentNullException(nameof(candidateId));

            var candidate = await _repo.GetCandidateByIdAsync(candidateId, track: true);

            var mappedCandidate = _mapper.Map<ResponseGetCandidateProfileJson>(candidate);

            return mappedCandidate;
        }
    }
}
