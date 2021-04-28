using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Core.Services.Transactions;
using Bank.Core.Validators;
using Bank.Core.ViewModels.Transactions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bank.Web.Controllers
{
    [Authorize(Roles = "Admin, Cashier")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<IActionResult> TransactionDetails(int accountId)
        {
            var model = await _transactionService.GetAmountByIdAsync(accountId, 0, 20).ConfigureAwait(false);
            return View(model);
        }

        public async Task<PartialViewResult> SingleTransactionPartial(int accountId, int skip)
        {
            var viewModel = await _transactionService.GetAmountByIdAsync(accountId, skip, 20).ConfigureAwait(false);
            return PartialView("Transactions/_TransactionDetailsTableBody", viewModel);
        }

        public IActionResult Deposit()
        {
            var model = new DepositViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(DepositViewModel model)
        {
            if (ModelState.IsValid)
                await _transactionService.SaveDepositAsync(model).ConfigureAwait(false);
            
            return View(model);
        }

        public IActionResult Withdraw()
        {
            var model = new WithdrawViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(WithdrawViewModel model)
        {
            if (ModelState.IsValid)
               await _transactionService.SaveWithdrawAsync(model).ConfigureAwait(false);
            
            return View(model);
        }

        public IActionResult Transfer()
        {
            var model = new TransferViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(TransferViewModel model)
        {
            if (ModelState.IsValid)
                await _transactionService.SaveTransferAsync(model).ConfigureAwait(false);

            return View(model);
        }
    }
}
