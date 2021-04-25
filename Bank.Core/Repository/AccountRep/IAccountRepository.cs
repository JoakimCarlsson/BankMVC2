using Bank.Core.Model;
using Bank.Core.Repository.Base;

namespace Bank.Core.Repository.AccountRep
{
    public interface IAccountRepository : IAsyncRepository<Account>
    {
        
    }
}