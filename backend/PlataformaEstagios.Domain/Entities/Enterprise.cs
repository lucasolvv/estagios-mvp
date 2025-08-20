namespace PlataformaEstagios.Domain.Entities
{
    public class Enterprise : EntityBase
    {
        public Guid EnterpriseIdentifier { get; set; } = Guid.NewGuid();
        public Guid UserIdentifier { get; set; }
        public string? EnterpriseName { get; set; }
        public string? Cnpj { get; set; }
        public string? ActivityArea { get; set; }
        public Address? Address { get; set; }
        public ICollection<Vacancy>? Vacancies { get; set; } = new List<Vacancy>();
    }
}
