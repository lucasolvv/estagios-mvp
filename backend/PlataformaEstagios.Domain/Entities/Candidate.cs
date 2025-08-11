namespace PlataformaEstagios.Domain.Entities
{
    public class Candidate
    {
        public Guid UserIdentifier { get; set; } = Guid.NewGuid();
        public User? User { get; set; }

        public int CandidateId { get; set; }

        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? CourseName { get; set; }

        public Address? Address { get; set; }
        public ICollection<Application>? Applications { get; set; }
    }
}
