using System;
using System.Linq;
using System.Threading.Tasks;
using Bank.Data.Models;
using Bank.MoneyLaundererBatch.Services;

namespace Bank.MoneyLaundererBatch
{
    public class Application
    {
        private readonly IMoneyLaundererService _moneyLaundererService;

        public Application(IMoneyLaundererService moneyLaundererService)
        {
            _moneyLaundererService = moneyLaundererService;
        }
        
        public async Task RunAsync()
        {
            
        }
    }
}