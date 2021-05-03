using System.Linq;
using System.Threading.Tasks;
using Bank.Data.Data;
using Bank.Data.Repositories.Base;

namespace Bank.Data.Repositories.Transaction
{
    public class TransactionRepository : BaseRepository<Models.Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IQueryable<Models.Transaction>> ListAllByAccountIdAsync(int accountId)
        {
            return _dbContext.Transactions.Where(i => i.AccountId == accountId).AsQueryable();
        }
    }
}