using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bank.Data.Repositories.Account;
using Bank.Data.Repositories.Customer;
using Bank.Web.ViewModels.Accounts;

namespace Bank.Web.Services.Account
{
    public interface IAccountService
    {
        public Task SaveAsync(AccountEditViewModel model);
        public Task<AccountEditViewModel> GetEditViewModelFromId(int id);
        public Task<List<AccountDisponentViewModel>> GetAccountUsers(int id);
    }
}