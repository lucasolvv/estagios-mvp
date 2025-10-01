namespace PlataformaEstagios.Application.UseCases.Application.Update
{
    public interface IUpdateApplicationUseCase
    {
        Task<(bool Success, string? Error)> UpdateApplicationStatus(Guid applicationId, string status, CancellationToken ct = default);
    }
}
