namespace PlataformaEstagios.Domain.Repositories.Interview
{
    public interface IInterviewWriteOnlyRepository
    {
        Task AddAsync(Entities.Interview entity);
    }
}
