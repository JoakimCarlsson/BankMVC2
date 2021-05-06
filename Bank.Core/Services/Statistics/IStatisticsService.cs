using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Core.ViewModels;
using Bank.Core.ViewModels.Statistics;

namespace Bank.Core.Services.Statistics
{
    public interface IStatisticsService
    {
        public IndexViewModel GetBaseStatistics();
        public Task<CountryStatisticsViewModel> GetCountryStatisticsAsync();
        public Task<IEnumerable<TopCustomerViewModel>> GetTopCustomersByCountry(int amount, string country);
    }
}
