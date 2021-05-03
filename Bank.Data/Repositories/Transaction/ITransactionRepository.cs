using System.Linq;
using System.Threading.Tasks;
using Bank.Data.Repositories.Base;

namespace Bank.Data.Repositories.Transaction
{
    public interface ITransactionRepository : IAsyncRepository<Models.Transaction>
    {
        Task<IQueryable<Models.Transaction>> ListAllByAccountIdAsync(int accountId);
    }
}
