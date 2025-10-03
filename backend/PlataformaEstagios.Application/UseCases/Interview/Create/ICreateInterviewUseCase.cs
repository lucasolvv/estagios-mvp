using PlataformaEstagios.Communication.Requests;

namespace PlataformaEstagios.Application.UseCases.Interview.Create
{
    public interface ICreateInterviewUseCase
    {
        Task<(bool Success, int Code, string? Error)> ExecuteAsync(
           Guid applicationId,
           RequestCreateScheduleInterviewJson request,
           Guid? enterpriseIdentifier);
    }
}
