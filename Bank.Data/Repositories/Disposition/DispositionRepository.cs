using System.Linq;
using System.Threading.Tasks;
using Bank.Data.Data;
using Bank.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Bank.Data.Repositories.Disposition
{
    public class DispositionRepository : BaseRepository<Models.Disposition>, IDispositionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DispositionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IQueryable<Models.Disposition>> ListAllByCustomerIdAsync(int customerId)
        {
            return Task.FromResult(_dbContext.Dispositions.Include(a => a.Account).Where(i => i.CustomerId == customerId).AsQueryable());
        }
    }
}