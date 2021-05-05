using Bank.Core.Services.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Web.Areas.ViewComponents.BankStatistics
{
    public class BankStatisticsComponent : ViewComponent
    {
        private readonly IStatisticsService _homeService;

        public BankStatisticsComponent(IStatisticsService homeService)
        {
            _homeService = homeService;
        }

        public IViewComponentResult Invoke()
        {
            var model = _homeService.GetStatistics();
            return View("/Views/Shared/Components/Home/BankStatistics.cshtml", model);
        }
    }
}
