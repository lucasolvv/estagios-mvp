namespace PlataformaEstagios.Communication.Requests
{
    public class RequestCreateApplicationJson
    {
        /// <summary>
        /// Identificador da vaga na qual o candidato está se inscrevendo.
        /// </summary>
        public Guid VacancyId { get; set; }

        /// <summary>
        /// Identificador do candidato (vem das claims após login).
        /// </summary>
        public Guid CandidateIdentifier { get; set; }
    }
}
