using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;
using Bank.Core.Services.Transactions;
using Bank.Core.ViewModels.Transactions;
using Bank.Data.Models;
using Bank.Data.Repositories.Account;

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
            var model = await _transactionService.GetAmountByIdAsync(id, offset, limit).ConfigureAwait(false);
            return Ok(model);
        }
    }
}
