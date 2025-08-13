using PlataformaEstagios.Domain.Enums;
namespace PlataformaEstagios.Domain.Entities
{
    public class Application : EntityBase
    {
        public Guid ApplicationIdentifier { get; set; }
        public Guid VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }
        public Guid CandidateIdentifier { get; set; }
        public Candidate? Candidate { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    }
}
