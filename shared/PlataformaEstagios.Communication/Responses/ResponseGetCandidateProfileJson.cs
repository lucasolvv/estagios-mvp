namespace PlataformaEstagios.Communication.Responses
{
    public sealed class ResponseGetCandidateProfileJson
    {
        public Guid CandidateId { get; init; }
        public string? FullName { get; init; }
        public string? Phone { get; init; }
        public string? Bio { get; init; }
        public string? ProfilePictureUrl { get; init; } // público (wwwroot)
        public bool HasResume { get; init; }            // privado (baixado por endpoint)
    }
}
