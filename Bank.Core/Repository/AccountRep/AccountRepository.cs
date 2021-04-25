using Bank.Core.Data;
using Bank.Core.Model;
using Bank.Core.Repository.Base;

namespace Bank.Core.Repository.AccountRep
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}