namespace PlataformaEstagios.Domain.Entities
{
    public class Interview : EntityBase
    {
        public Guid InterviewIdentifier { get; set; } = Guid.NewGuid();

        // FK para a candidatura
        public Guid ApplicationIdentifier { get; set; }
        public Application Application { get; set; } = null!; // nav obrigatória

        // Dados do agendamento
        public DateTimeOffset StartAt { get; set; }          // data/hora (com offset)
        public int DurationMinutes { get; set; }             // duração em minutos

        public string? Location { get; set; }                // presencial (opcional)
        public string? MeetingLink { get; set; }             // remoto (opcional)
        public string? Notes { get; set; }                   // observações (opcional)

        // Metadados simples
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
