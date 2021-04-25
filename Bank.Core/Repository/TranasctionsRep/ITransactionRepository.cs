using Bank.Core.Model;
using Bank.Core.Repository.Base;

namespace Bank.Core.Repository.TranasctionsRep
{
    public interface ITransactionRepository : IAsyncRepository<Transaction>
    {
    }
}
