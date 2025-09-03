using PlataformaEstagios.Domain.Enums;

namespace PlataformaEstagios.Communication.Requests
{
    public class RequestCreateVacancyJson
    {
        public Guid EnterpriseIdentifier { get; set; }   // vindo da rota, mas valido aqui tb
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // NOVOS CAMPOS
        public string Location { get; set; } = "Remoto - Brasil";
        public JobFunction JobFunction { get; set; }
        public List<string> RequiredSkills { get; set; } = new();   // ex.: ["C#", "Git", "SQL"]
        public DateTime? ExpiresAtUtc { get; set; }                 // opcional
    }
}
