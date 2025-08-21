namespace PlataformaEstagios.Domain.Repositories.Enterprise
{
    public interface IEnterpriseWriteOnlyRepository
    {
        Task CreateEnterprise(Entities.Enterprise enterprise);
    }
}
