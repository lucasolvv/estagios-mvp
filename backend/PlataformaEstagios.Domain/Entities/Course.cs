namespace PlataformaEstagios.Domain.Entities
{
    public class Course : EntityBase
    {
        public Guid CourseIdentifier { get; set; }
        public string Name { get; set; }
        public string? Institution { get; set; }
        public ICollection<Candidate>? Candidates { get; set; }
    }
}
