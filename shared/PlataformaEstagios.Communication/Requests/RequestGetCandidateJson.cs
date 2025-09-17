using System;

namespace PlataformaEstagio.Web.Components.Services.Candidate
{
    // Request para PUT /profile (igual ao contrato backend, base64 ou data URI)
    public sealed class RequestUpdateCandidateProfileJson
    {
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Course { get; set; }
        public AddressDto? Address { get; set; }
        public string? ProfilePictureBase64 { get; set; } // data URI ou base64 puro
        public string? ResumeBase64 { get; set; }         // data URI ou base64 puro (PDF)
    }

    public sealed class AddressDto
    {
        public string? Street { get; set; }
        public string? Complement { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? Uf { get; set; }  // 2 letras
        public string? Cep { get; set; } // 8 dígitos numéricos
    }

    // Resumo do Candidate retornado no GET (campos que a UI usa)
    public sealed class CandidateDto
    {
        public Guid CandidateIdentifier { get; set; }
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Course { get; set; }
        public AddressDto? Address { get; set; }
        public string? ProfilePicturePath { get; set; } // público (wwwroot/uploads/...)
    }
}
