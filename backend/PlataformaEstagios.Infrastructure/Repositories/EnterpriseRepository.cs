using PlataformaEstagios.Domain.Repositories.Enterprise;
using PlataformaEstagios.Infrastructure.DataAccess;

namespace PlataformaEstagios.Infrastructure.Repositories
{
    public class EnterpriseRepository : IEnterpriseWriteOnlyRepository
    {
        private readonly AppDbContext _dbcontext;
        public EnterpriseRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task CreateEnterprise(Domain.Entities.Enterprise enterprise)
        {
            await _dbcontext.Enterprises.AddAsync(enterprise);
        }
    }
}
