using Bank.Web.Services.Account;
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
        
        public IActionResult Index()
        {
            return View();
        }
    }
}