using System.Threading.Tasks;
using Bank.API.Services;
using Bank.API.Services.Transactions;
using Bank.API.ViewModels;
using Bank.API.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Bank.API.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public AccountsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        
        [Route("{id}, {limit}, {offset}")]
        [HttpGet]
        public async Task<ActionResult<TransactionDetailsListViewModel>> Get(int id, int limit, int offset)
        {
            var model = await _transactionService.GetTransactions(id, limit, offset).ConfigureAwait(false);
            return Ok(model);
        }
    }
}