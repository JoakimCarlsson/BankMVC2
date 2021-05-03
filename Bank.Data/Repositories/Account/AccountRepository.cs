using Bank.Data.Data;
using Bank.Data.Repositories.Base;

namespace Bank.Data.Repositories.Account
{
    public class AccountRepository : BaseRepository<Models.Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}