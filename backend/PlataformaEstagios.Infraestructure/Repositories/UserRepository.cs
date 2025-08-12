using PlataformaEstagios.Domain.Repositories.User;
using PlataformaEstagios.Infrastructure.DataAccess;

namespace PlataformaEstagios.Infrastructure.Repositories
{
    public class UserRepository : IUserWriteOnlyRepository
    {
        private readonly AppDbContext _dbcontext;
        public UserRepository(AppDbContext db)
        {
            _dbcontext = db;
        }
        public async Task CreateUser(Domain.Entities.User user)
        {
            await _dbcontext.AddAsync(user);
        }
    }
}
