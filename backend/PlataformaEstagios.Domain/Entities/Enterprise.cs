namespace PlataformaEstagios.Domain.Entities
{
    public class Enterprise : EntityBase
    {
        public Guid EnterpriseIdentifier { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string? EnterpriseName { get; set; }
        public string? Cnpj { get; set; }
        public string? ActivityArea { get; set; }
        public Guid? AddressIdentifier { get; set; }
        public Address? Address { get; set; }
        public ICollection<Vacancy>? Vacancys { get; set; } = new List<Vacancy>();
    }
}
