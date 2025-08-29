namespace PlataformaEstagios.Communication.Requests
{
    public class RequestCreateVacancyJson
    {
        public Guid EnterpriseIdentifier { get; set; }   // vindo da rota, mas valido aqui tb
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
