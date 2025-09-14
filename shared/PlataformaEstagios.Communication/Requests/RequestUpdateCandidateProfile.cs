using PlataformaEstagios.Communication.Requests.Files;

namespace PlataformaEstagios.Communication.Requests
{
    public sealed class RequestUpdateCandidateProfile
    {
        public Guid CandidateId { get; init; }

        // Ex.: dados textuais do perfil
        public string? FullName { get; init; }
        public string? Phone { get; init; }
        public string? Bio { get; init; }

        // Uploads opcionais
        public FileUpload? ProfilePicture { get; init; }
        public FileUpload? Resume { get; init; }
    }
}
