using System.Threading.Tasks;
using Bank.Web.Services.Account;
using Bank.Web.ViewModels.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        public async Task<IActionResult> Edit(int accountId)
        {
            var model = await _accountService.GetEditViewModelFromId(accountId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccountEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _accountService.SaveAsync(model);
                return RedirectToAction(nameof(Edit),new {accountId = model.AccountId});
            }

            model.Disponents = await _accountService.GetAccountUsers(model.AccountId);
            return View(model);
        }
    }
}