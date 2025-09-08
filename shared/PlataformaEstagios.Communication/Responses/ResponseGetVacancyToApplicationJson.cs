using PlataformaEstagios.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlataformaEstagios.Communication.Responses
{
    public class ResponseGetVacancyToApplicationJson
    {
        public Guid VacancyIdentifier { get; set; }
        public string? EnterpriseName { get; set; }
        [Required, MinLength(3)]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required, MinLength(3)]
        public string Location { get; set; }
        [Required]
        public JobFunction JobFunction { get; set; }
        public List<string> RequiredSkills { get; set; }
        public DateTime? ExpiresAtUtc { get; set; }
        public int ApplicantsCount { get; set; }
        public DateTime OpenedAt { get; set; }
    }
}
