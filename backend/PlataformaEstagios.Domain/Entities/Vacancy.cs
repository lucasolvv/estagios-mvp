namespace PlataformaEstagios.Domain.Entities
{
    public class Vacancy
    {
        public Guid VacancyIdentifier { get; set; }
        public Guid EnterpriseIdentifier { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Application>? Applications { get; set; }
    }
}
