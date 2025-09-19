using PlataformaEstagios.Communication;
using System;

namespace PlataformaEstagio.Web.Components.Services.Candidate
{
    // Request para PUT /profile (igual ao contrato backend, base64 ou data URI)
    public sealed class RequestUpdateCandidateProfileJson
    {
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BioResume { get; init; }
        public string? Course { get; set; }
        public AddressDto? Address { get; set; }
        public string? ProfilePictureBase64 { get; set; } // data URI ou base64 puro
        public string? ResumeBase64 { get; set; }         // data URI ou base64 puro (PDF)
    }
}
