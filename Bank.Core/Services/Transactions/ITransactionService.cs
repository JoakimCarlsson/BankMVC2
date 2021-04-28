using System.Threading.Tasks;
using Bank.Core.ViewModels.Customers;
using Bank.Core.ViewModels.Transactions;

namespace Bank.Core.Services.Transactions
{
    public interface ITransactionService
    {
        public Task<TransactionDetailsListViewModel> GetByAllIdAsync(int accountId);
        public Task<TransactionDetailsListViewModel> GetAmountByIdAsync(int accountId, int skip, int take);
        public Task SaveDeposit(DepositViewModel model);
        public Task SaveTransferAsync(TransferViewModel model);
    }
}