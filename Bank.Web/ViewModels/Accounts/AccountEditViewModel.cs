using System.Collections.Generic;

namespace Bank.Web.ViewModels.Accounts
{
    public class AccountEditViewModel
    {
        public int AccountId { get; set; }
        public int NewCustomerId { get; set; }
        public IEnumerable<AccountDisponentViewModel> Disponents { get; set; }
    }
}