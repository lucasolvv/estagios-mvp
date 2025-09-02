using PlataformaEstagios.Domain.Enums;

namespace PlataformaEstagios.Communication.Responses
{
    public class ResponseCreateVacancyJson
    {
        public Guid VacancyIdentifier { get; set; }
        public string Title { get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }

        // NOVOS CAMPOS (de retorno)
        public string? Description { get; set; }
        public string Location { get; set; } = default!;
        public JobFunction JobFunction { get; set; }
        public List<string> RequiredSkills { get; set; } = new();
        public DateTime? PublishedAtUtc { get; set; }
        public DateTime? ExpiresAtUtc { get; set; }
    }
}
