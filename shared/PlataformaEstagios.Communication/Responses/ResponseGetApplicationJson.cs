using PlataformaEstagios.Domain.Entities;
using PlataformaEstagios.Domain.Enums;

namespace PlataformaEstagios.Communication.Responses
{
    public class ResponseGetApplicationJson
    {
        public Guid ApplicationIdentifier { get; set; }
        public Guid VacancyIdentifier { get; set; }        
        public string TituloVaga { get; set; }
        public string NomeEmpresa { get; set; }
        public DateTime DataCandidatura { get; set; }
        public string Status { get; set; }
    }
}
