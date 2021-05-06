using System.Threading.Tasks;
using Bank.Core.Services.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Web.Areas.ViewComponents.BankStatistics
{
    public class CountryStatisticsComponent : ViewComponent
    {
        private readonly IStatisticsService _statisticsService;

        public CountryStatisticsComponent(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _statisticsService.GetCountryStatisticsAsync();
            return View("/Views/Shared/Components/Home/CountryStatistics.cshtml", model);
        }
    }
}
