namespace PlataformaEstagios.Domain.Entities
{
    public class Address : EntityBase
    {
        public Guid AddressIdentifier { get; set; } = Guid.NewGuid();
        public string? Street { get; set; }
        public string? Complement { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? UF { get; set; }
        public string? CEP { get; set; }
    }
}
