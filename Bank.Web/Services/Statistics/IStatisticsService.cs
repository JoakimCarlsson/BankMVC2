using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Web.ViewModels;
using Bank.Web.ViewModels.Statistics;

namespace Bank.Web.Services.Statistics
{
    public interface IStatisticsService
    {
        public IndexViewModel GetBaseStatistics();
        public Task<CountryStatisticsViewModel> GetCountryStatisticsAsync();
        public Task<IEnumerable<TopCustomerViewModel>> GetTopCustomersByCountry(int amount, string country);
    }
}
