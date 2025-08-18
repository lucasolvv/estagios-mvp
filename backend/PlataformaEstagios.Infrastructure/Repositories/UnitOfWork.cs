using PlataformaEstagios.Domain.Repositories;
using PlataformaEstagios.Infrastructure.DataAccess;

namespace PlataformaEstagios.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbcontext;

        public UnitOfWork(AppDbContext dbContext) => _dbcontext = dbContext;

        public async Task Commit() => await _dbcontext.SaveChangesAsync();

    }
}
