namespace PlataformaEstagios.Communication.Responses
{
    public sealed record ResponseVacancyListItem(
     Guid VacancyIdentifier,
     string Title,
     DateTime OpenedAt,
     int Applicants,
     string Status);
}
