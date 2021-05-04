using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Core.Services.Home;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Web.Areas.ViewComponents.BankStatistics
{
    public class BankStatisticsComponent : ViewComponent
    {
        private readonly IHomeService _homeService;

        public BankStatisticsComponent(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public IViewComponentResult Invoke()
        {
            var model = _homeService.GetStats();
            return View("/Views/Shared/Components/Home/BankStatistics.cshtml", model);
        }
    }
}
