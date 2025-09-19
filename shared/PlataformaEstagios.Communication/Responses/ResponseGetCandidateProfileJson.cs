using PlataformaEstagio.Web.Components.Services.Candidate;

namespace PlataformaEstagios.Communication.Responses
{
    public sealed class ResponseGetCandidateProfileJson
    {
        public Guid CandidateId { get; set; }
        public string? FullName { get; set; }
        public string? CourseName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BioResume { get; set; }
        public AddressDto? Address { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? ResumePath { get; set; }

    }
}
