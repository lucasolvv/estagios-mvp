using AutoMapper;
using PlataformaEstagios.Communication.Responses;
using PlataformaEstagios.Domain.Repositories.Interview;

namespace PlataformaEstagios.Application.UseCases.Interview.Get
{
    public class GetInterviewUseCase : IGetInterviewUseCase
    {
        private readonly IInterviewReadOnlyRepository _interviewReadRepo;
        private readonly IMapper _mapper;

        public GetInterviewUseCase(IInterviewReadOnlyRepository interviewReadRepo, IMapper mapper)
        {
            _interviewReadRepo = interviewReadRepo;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ResponseGetInterviewItemJson>?> GetInterviewsByApplicationIdAsync(Guid applicationId)
        {
            var interviews = await _interviewReadRepo.GetByApplicationIdAsync(applicationId);
            return _mapper.Map<IReadOnlyList<ResponseGetInterviewItemJson>>(interviews);
        }
    }
}
