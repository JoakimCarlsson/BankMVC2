using System.Threading.Tasks;
using Bank.Web.ViewModels.Transactions;

namespace Bank.Web.Services.Transactions
{
    public interface ITransactionService
    {
        public Task<TransactionDetailsListViewModel> GetAmountByIdAsync(int accountId, int skip, int take);
        public Task<TransactionResultViewModel> SaveTransaction(TransactionBaseViewModel model);
        public Task<TransactionConfirmationViewModel> GetConfirmation(int transactionId);
    }
}