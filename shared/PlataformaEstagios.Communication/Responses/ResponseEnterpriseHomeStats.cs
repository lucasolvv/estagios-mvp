namespace PlataformaEstagios.Communication.Responses
{
    // Communication/Responses/EnterpriseHomeStatsResponse.cs
    public sealed record ResponseEnterpriseHomeStats(
        int OpenVacancies,
        int Applications,
        int Interviews,         // por enquanto 0 se ainda não tiver tabela
        double FillRate);       // 0..1
}
