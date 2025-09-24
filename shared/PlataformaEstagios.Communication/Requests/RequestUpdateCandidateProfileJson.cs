// PlataformaEstagios.Communication/Requests/RequestUpdateCandidateProfileJson.cs
namespace PlataformaEstagios.Communication.Requests
{
    public sealed class RequestUpdateCandidateProfileJson
    {
        // Dados básicos (opcionais)
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Course { get; set; }
        public RequestUpdateAddressJson? Address { get; set; }
        public string? BioResume { get; set; }

        // Arquivos (Base64) — sem FileName
        public string? ProfilePictureBase64 { get; set; }
        public string? ResumeBase64 { get; set; }
    }

    public sealed class RequestUpdateAddressJson
    {
        public string? Street { get; set; }
        public string? Complement { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? UF { get; set; }
        public string? CEP { get; set; }
    }
}
