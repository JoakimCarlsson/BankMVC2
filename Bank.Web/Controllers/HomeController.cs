using Bank.Core.Services.Home;
using Bank.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Bank.Web.Controllers
{
    [Authorize(Roles = "Admin, Cashier")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }

        [ResponseCache(Duration = 30)]
        public IActionResult Index()
        {
            return View(_homeService.GetStats());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
