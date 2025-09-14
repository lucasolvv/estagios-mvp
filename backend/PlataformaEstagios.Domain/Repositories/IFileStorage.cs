namespace PlataformaEstagios.Domain.Repositories
{
    public interface IFileStorage
    {
        // retorna caminho relativo para consumo externo (ex.: /uploads/candidates/{id}/profile.jpg)
        Task<string> SavePublicProfilePictureAsync(Guid candidateId, Stream content, string fileName, CancellationToken ct);

        // retorna caminho privado (ex.: Storage/Candidates/{id}/resume.pdf) – não público
        Task<string> SavePrivateResumeAsync(Guid candidateId, Stream content, string fileName, CancellationToken ct);

        // utilitário para abrir stream do currículo privado
        Task<Stream> OpenPrivateAsync(string privatePath, CancellationToken ct);
    }
}
