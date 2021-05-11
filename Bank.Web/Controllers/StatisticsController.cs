using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Web.Services.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Web.Controllers
{
    [Authorize(Roles = "Admin, Cashier")]
    public class StatisticsController : Controller
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> Index(string country)
        {
            var model = await _statisticsService.GetTopCustomersByCountry(10, country).ConfigureAwait(false);
            return View(model);
        }
    }
}
