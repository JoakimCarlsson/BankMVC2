using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Core.Services.User;
using Bank.Core.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Bank.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        { 
            _userService = userService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            return View(await _userService.GetAllUsersAsync().ConfigureAwait(false));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await _userService.GetUserEdit(id).ConfigureAwait(false);
            model.AllRoles = await _userService.GetUserRoles(id).ConfigureAwait(false);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _userService.SaveUserAsync(model).ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }

            model.AllRoles = await _userService.GetUserRoles(model.Id).ConfigureAwait(false);
            return View(model);
        }
    }
}
