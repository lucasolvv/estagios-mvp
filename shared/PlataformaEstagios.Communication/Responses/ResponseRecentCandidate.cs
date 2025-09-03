namespace PlataformaEstagios.Communication.Responses
{
    public sealed record ResponseRecentCandidate(
     Guid ApplicationIdentifier,
     string CandidateName,
     string Course,
     string VacancyTitle,
     DateTime AppliedAt);
}
