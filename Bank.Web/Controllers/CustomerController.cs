using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Bank.Core.Services.Customers;

namespace Bank.Web.Controllers
{
    [Authorize(Roles = "Admin, Cashier")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> CustomerDetails(int id)
        {
            var model = await _customerService.GetByIdAsync(id).ConfigureAwait(false);
            if (model is null)
                return View("_Error");

            return View(model);
        }

        public async Task<IActionResult> Search(string q, int page = 1, int pageSize = 50)
        {
            var model = await _customerService.GetPagedSearchAsync(q, page, pageSize).ConfigureAwait(false);
            return View(model);
        }
    }
}
