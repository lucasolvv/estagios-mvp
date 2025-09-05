using PlataformaEstagios.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlataformaEstagios.Communication.Requests
{
    public class RequestUpdateVacancyJson
    {
        public Guid VacancyIdentifier { get; set; } // opcional, se o backend exigir
        [Required, MinLength(3)]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required, MinLength(3)]
        public string Location { get; set; } = "Remoto - Brasil";
        [Required]
        public JobFunction JobFunction { get; set; }
        public List<string> RequiredSkills { get; set; } = new();
        public DateTime? ExpiresAtUtc { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
