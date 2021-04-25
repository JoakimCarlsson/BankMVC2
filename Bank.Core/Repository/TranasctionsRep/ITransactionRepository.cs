using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Core.Model;
using Bank.Core.Repository.Base;

namespace Bank.Core.Repository.TranasctionsRep
{
    public interface ITransactionRepository : IAsyncRepository<Transaction>
    {
        Task<List<Transaction>> ListAllByAccountIdAsync(int accountId);
    }
}
