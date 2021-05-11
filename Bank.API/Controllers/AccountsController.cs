using Microsoft.AspNetCore.Mvc;

namespace Bank.API.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AccountsController : ControllerBase
    {

        //[Route("{id}, {limit}, {offset}")]
        //[HttpGet]
        //public async Task<ActionResult<TransactionDetailsListViewModel>> Get(int id, int limit, int offset)
        //{
        //    var model = await _transactionService.GetTransactions(id, offset, limit).ConfigureAwait(false);
        //    return Ok(model);
        //}
    }
}
