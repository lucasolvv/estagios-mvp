using PlataformaEstagios.Communication.Responses;

namespace PlataformaEstagios.Application.UseCases.Interview.Get
{
    public interface IGetInterviewUseCase
    {
        Task<IReadOnlyList<ResponseGetInterviewItemJson>?> GetInterviewsByApplicationIdAsync(Guid applicationId);
        Task<IReadOnlyList<ResponseGetInterviewItemJson>?> GetInterviewsByEnterpriseIdAsync(Guid enterpriseId);
    }
}
