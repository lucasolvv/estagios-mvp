namespace PlataformaEstagios.Domain.Entities
{
    public class Candidate : EntityBase
    {
        public Guid UserIdentifier { get; set; }

        public Guid CandidateIdentifier { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public DateOnly? BirthDate { get; set; }
        public string? BioResume { get; set; }
        public string? CourseName { get; set; }

        public Address? Address { get; set; }
        public ICollection<Application>? Applications { get; set; }

        public string? ProfilePicturePath { get; private set; }
        public string? ResumePath { get; private set; }

        public void SetProfilePicture(string? relativePath) => ProfilePicturePath = relativePath;
        public void SetResume(string? privatePath) => ResumePath = privatePath;
    }
}
