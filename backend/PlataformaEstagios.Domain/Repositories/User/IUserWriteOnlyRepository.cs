namespace PlataformaEstagios.Domain.Repositories.User
{
    public interface IUserWriteOnlyRepository
    {
        public Task CreateUser(Entities.User user);
    }
}
