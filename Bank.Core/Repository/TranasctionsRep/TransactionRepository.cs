using Bank.Core.Data;
using Bank.Core.Model;
using Bank.Core.Repository.Base;

namespace Bank.Core.Repository.TranasctionsRep
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}