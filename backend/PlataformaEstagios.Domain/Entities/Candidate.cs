namespace PlataformaEstagios.Domain.Entities
{
    public class Candidate : EntityBase
    {
        public Guid UserIdentifier { get; set; }

        public Guid CandidateIdentifier { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public DateOnly? BirthDate { get; set; }

        public string? CourseName { get; set; }

        public Address? Address { get; set; }
        public ICollection<Application>? Applications { get; set; }
    }
}
