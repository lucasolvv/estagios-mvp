using Microsoft.EntityFrameworkCore;
using PlataformaEstagios.Domain.Repositories.User;
using PlataformaEstagios.Infrastructure.DataAccess;

namespace PlataformaEstagios.Infrastructure.Repositories
{
    public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
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

        public async Task<Domain.Entities.User?> GetByEmailOrNicknameAsync(string emailOrNickname, CancellationToken ct)
        {
            return await _dbcontext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == emailOrNickname || u.Nickname == emailOrNickname, ct);
        }

        public async Task<bool> VerifyUserExistsByEmailOrUsername(string emailOrNickname, CancellationToken ct)
        {
            return await _dbcontext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == emailOrNickname || u.Nickname == emailOrNickname, ct);
        }
    }
}
