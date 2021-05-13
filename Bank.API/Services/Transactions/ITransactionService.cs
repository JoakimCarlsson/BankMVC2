using System.Threading.Tasks;
using Bank.API.ViewModels.Transactions;

namespace Bank.API.Services.Transactions
{
    public interface ITransactionService
    {
        Task<TransactionDetailsListViewModel> GetTransactions(int id, int limit, int offset);
    }
}   