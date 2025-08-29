using PlataformaEstagios.Domain.Enums;

namespace PlataformaEstagios.Communication.Responses
{
    public class ResponseLoginJson
    {
        public string AccessToken { get; set; } = null!;
        public DateTime ExpiresAtUtc { get; set; }

        // handy extras for the frontend
        public Guid UserIdentifier { get; set; }
        public string Nickname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public UserType UserType { get; set; }
        public Guid? UserTypeId { get; set; } // CandidateId or EnterpriseId

    }
}
