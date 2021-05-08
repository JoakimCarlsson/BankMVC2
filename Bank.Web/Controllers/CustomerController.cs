using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Bank.Core.Services.Customers;
using Bank.Core.ViewModels.Customers;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<IActionResult> Index(string q, int page = 1, int pageSize = 50)
        {
            var model = await _customerService.GetPagedSearchAsync(q, page, pageSize).ConfigureAwait(false);
            return View(model);
        }

        public IActionResult Register()
        {
            var model = new CustomerRegisterViewModel {Genders = GetGenders()};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CustomerRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _customerService.SaveCustomerAsync(model).ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }

            model.Genders = GetGenders();
            return View(model);
        }

        private List<SelectListItem> GetGenders()
        {
            var tmpList = new List<SelectListItem>
            {
                new() {Value = "male", Text = "Male"},
                new() {Value = "female", Text = "Female"}
            };

            return tmpList;
        }
    }
}
