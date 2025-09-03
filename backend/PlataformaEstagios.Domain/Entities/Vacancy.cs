using PlataformaEstagios.Domain.Enums;

namespace PlataformaEstagios.Domain.Entities
{
    public class Vacancy
    {
        public Guid VacancyIdentifier { get; set; }
        public Guid EnterpriseIdentifier { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        // NOVOS CAMPOS
        public string Location { get; set; } = "Remoto - Brasil";
        public JobFunction JobFunction { get; set; }
        public string RequiredSkillsCsv { get; set; } = "";   // armazenamos como "C#, Git, SQL"
        public DateTime? PublishedAtUtc { get; set; }
        public DateTime? ExpiresAtUtc { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Application>? Applications { get; set; }
    }
}
