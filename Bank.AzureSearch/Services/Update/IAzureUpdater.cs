using System.Threading.Tasks;
using Bank.Data.Data;
using Bank.Data.Models;
using Bank.Data.Repositories.Transaction;

namespace Bank.AzureSearchService.Services
{
    public interface IAzureUpdater
    {
        public Task RunCustomerUpdateBatchAsync();
        public Task AddOrUpdateCustomerInAzure(Customer customer);
    }
}