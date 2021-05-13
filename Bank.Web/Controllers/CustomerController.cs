using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Bank.Web.Services.Customers;
using Bank.Web.ViewModels.Customers;
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

        public async Task<IActionResult> Index(string q, string sortField, string sortOrder, int page = 1, int pageSize = 50)
        {
            // var model = await _customerService.GetPagedSearchAsync(q, page, pageSize).ConfigureAwait(false);
            var model = await _customerService.GetAzurePagedSearchAsync(q, sortField, sortOrder, page, pageSize).ConfigureAwait(false);
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

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _customerService.GetCustomerEditAsync(id).ConfigureAwait(false);
            model.Genders = GetGenders(model.SelectedGender);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _customerService.SaveCustomerAsync(model).ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }

            model.Genders = GetGenders(model.SelectedGender);
            return View(model);
        }

        private List<SelectListItem> GetGenders(string selectedGender = "")
        {
            var tmpList = new List<SelectListItem>
            {
                new() {Value = "male", Text = "Male"},
                new() {Value = "female", Text = "Female"}
            };

            if (!string.IsNullOrEmpty(selectedGender))
            {
                foreach (var selectListItem in tmpList.Where(selectListItem => selectedGender == selectListItem.Value))
                {
                    selectListItem.Selected = true;
                }
            }

            return tmpList;
        }
    }
}
