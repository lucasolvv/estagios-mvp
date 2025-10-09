namespace PlataformaEstagios.Communication.Responses
{
    public class ResponseVacancyListItem
    {
        public Guid EnterpriseIdentifier { get; set; }
        public Guid VacancyIdentifier { get; set; }
        public string Title {get; set;}
        public DateTime OpenedAt {get; set;}
        public int Applicants {get; set;}
        public string Status {get; set;}
        public string? EnterpriseName {get; set;}
    }
}
