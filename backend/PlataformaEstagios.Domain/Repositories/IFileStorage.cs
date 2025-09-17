// PlataformaEstagios.Domain/Repositories/IFileStorage.cs
namespace PlataformaEstagios.Domain.Repositories
{
    public interface IFileStorage
    {
        // gera nome final: {slug}_profile.{ext} e {slug}_resume.{ext}
        Task<string> SavePublicProfilePictureBase64Async(
            Guid candidateId, string base64, string fileBaseName, CancellationToken ct);

        Task<string> SavePrivateResumeBase64Async(
            Guid candidateId, string base64, string fileBaseName, CancellationToken ct);

        Task<Stream> OpenPrivateAsync(string privatePath, CancellationToken ct);
    }
}
