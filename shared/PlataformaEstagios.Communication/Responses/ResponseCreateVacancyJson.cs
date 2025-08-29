namespace PlataformaEstagios.Communication.Responses
{
    public class ResponseCreateVacancyJson
    {
        public Guid VacancyIdentifier { get; set; }
        public string Title { get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
