using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Core.Model;
using Bank.Core.Repository.Base;

namespace Bank.Core.Repository.TranasctionsRep //todo fix me
{
    public interface ITransactionRepository : IAsyncRepository<Transaction>
    {
        Task<IQueryable<Transaction>> ListAllByAccountIdAsync(int accountId);
    }
}
