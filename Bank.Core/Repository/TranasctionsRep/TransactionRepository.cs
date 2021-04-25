using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Core.Data;
using Bank.Core.Model;
using Bank.Core.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Repository.TranasctionsRep
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IQueryable<Transaction>> ListAllByAccountIdAsync(int accountId)
        {
            return _dbContext.Transactions.Where(i => i.AccountId == accountId).AsQueryable();
        }
    }
}