using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;
using Bank.Core.Services.Transactions;
using Bank.Data.Models;
using Bank.Data.Repositories.Account;

namespace Bank.API.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AccountsController : ControllerBase
    {


        [Route("{id}, {limit}, {offset}")]
        [HttpGet]
        public async Task<ActionResult<Account>> Get(int id, int limit, int offset)
        {
            return NotFound();
        }
    }
}
