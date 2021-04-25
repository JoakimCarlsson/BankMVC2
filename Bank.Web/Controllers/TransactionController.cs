using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Core.Services.Transactions;

namespace Bank.Web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<IActionResult> TransactionDetails(int accountId)
        {
            var model = await _transactionService.GetAmountByIdAsync(accountId, 0, 20);
            return View(model);
        }

        public async Task<PartialViewResult> SingleTransactionPartial(int accountId, int skip)
        {
            var viewModel = await _transactionService.GetAmountByIdAsync(accountId, skip, 20);
            return PartialView("Transactions/_TransactionDetailsTableBody", viewModel);
        }
    }
}
